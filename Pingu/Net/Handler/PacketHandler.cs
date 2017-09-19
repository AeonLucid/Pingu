using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NLog;
using Pingu.Net.Message;

namespace Pingu.Net.Handler
{
    internal static class PacketHandler
    {

        private static Logger _logger;
        private static bool _initialised;

        private static Dictionary<string, string> _incomingNames;
        private static Dictionary<string, IMessageEvent> _incomingEvents;
        private static Dictionary<string, string> _outgoingNames;
        private static Dictionary<Type, Composer> _outgoingComposers;

        public static void Initialize()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _incomingNames = new Dictionary<string, string>();
            _incomingEvents = new Dictionary<string, IMessageEvent>();
            _outgoingNames = new Dictionary<string, string>();
            _outgoingComposers = new Dictionary<Type, Composer>();
            _initialised = true;

            RegisterIncoming();
            RegisterOutgoing();
            
            if (_incomingEvents.Count == _incomingNames.Count && _incomingEvents.Count != 0)
                _logger.Debug($"PacketHandler loaded {_incomingEvents.Count}/{_incomingNames.Count} events.");
            else if(_incomingEvents.Count != 0)
                _logger.Debug($"PacketHandler loaded {_incomingEvents.Count}/{_incomingNames.Count} events, please check your incoming packets.");
            else
                throw new Exception("0 Incoming events loaded, that seems wrong.");
            
            if (_outgoingComposers.Count == _outgoingNames.Count && _outgoingComposers.Count != 0)
                _logger.Debug($"PacketHandler loaded {_outgoingComposers.Count}/{_outgoingNames.Count} composers.");
            else if(_outgoingComposers.Count != 0)
                _logger.Debug($"PacketHandler loaded {_outgoingComposers.Count}/{_outgoingNames.Count} composers, please check your outgoing packets.");
            else
                throw new Exception("0 Composers loaded, that seems wrong.");
        }

        public static void HandleIncomingMessage(IncomingMessage incomingMessage, ClientHandler clientHandler)
        {
            if (_incomingEvents.ContainsKey(incomingMessage.Header))
            {
                _logger.Debug(clientHandler.Username == null
                    ? $"Received {incomingMessage.Header}."
                    : $"[{clientHandler.Username}] Received {incomingMessage.Header}.");
                _incomingEvents[incomingMessage.Header].HandleMessage(incomingMessage, clientHandler);
            }
            else
            {
                _logger.Warn($"Unknown incoming message '{incomingMessage.Header}' with {incomingMessage.Size} bytes..");
                _logger.Warn(incomingMessage.GetDocument().ToString(SaveOptions.DisableFormatting));
            }
        }

        public static T GetComposer<T>() where T : Composer
        {
            var type = typeof(T);
            if (_outgoingComposers.ContainsKey(type))
            {
                return (T) _outgoingComposers[type];
            }

            _logger.Error($"Tried to send {type} but it doesn't exist.");
            return default(T);
        }

        private static void RegisterIncoming()
        {
            if (!_initialised)
                throw new Exception("PacketHandler has not been initialised yet.");

            _incomingEvents.Clear();

            var type = typeof(IMessageEvent);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p)).ToArray();
            var filePaths = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Settings", "Packets"), "incoming.txt");

            foreach (var filePath in filePaths)
            {
                foreach (Match match in Regex.Matches(File.ReadAllText(filePath, Encoding.UTF8), @"^\s*([a-zA-Z0-9]+)=([a-zA-Z]+)", RegexOptions.Multiline))
                {
                    var name = match.Groups[1].Value;
                    var className = "MessageEvent" + name;
                    var header = match.Groups[2].Value;

                    _incomingNames.Add(header, name);

                    try
                    {
                        var messageType = types.FirstOrDefault(t => t.Name.Equals(className));
                        if (messageType == null)
                        {
                            _logger.Warn($"Incoming message handler for packet '{header}' not found.");

                            continue;
                        }
                        _incomingEvents.Add(header, (IMessageEvent)Activator.CreateInstance(messageType));
                    }
                    catch
                    {
                        _logger.Warn($"Failed to bind to {className}.");
                    }
                }
            }
        }

        private static void RegisterOutgoing()
        {
            if (!_initialised)
                throw new Exception("PacketHandler has not been initialised yet.");

            _outgoingComposers.Clear();

            var type = typeof(Composer);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p)).ToArray();
            var filePaths = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Settings", "Packets"), "outgoing.txt");

            foreach (var filePath in filePaths)
            {
                foreach (Match match in Regex.Matches(File.ReadAllText(filePath, Encoding.UTF8), @"^\s*([a-zA-Z0-9]+)=([a-zA-Z]+)", RegexOptions.Multiline))
                {
                    var name = match.Groups[1].Value;
                    var className = "Composer" + name;
                    var header = match.Groups[2].Value;

                    _outgoingNames.Add(header, name);

                    try
                    {
                        var messageType = types.FirstOrDefault(t => t.Name.Equals(className));
                        if (messageType == null)
                        {
                            _logger.Warn($"Outgoing composer for packet '{header}' not found.");

                            continue;
                        }

                        Composer composer = (Composer) Activator.CreateInstance(messageType);
                        composer.Header = header;

                        _outgoingComposers.Add(messageType, composer);
                    }
                    catch
                    {
                        _logger.Warn($"Failed to bind to {className}.");
                    }
                }
            }
        }
        

    }
}
