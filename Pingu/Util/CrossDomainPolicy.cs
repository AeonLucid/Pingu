namespace Pingu.Util
{
    class CrossDomainPolicy
    {

        public static string GetPolicy()
        {
            return "<?xml version=\"1.0\"?>\r\n<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n<cross-domain-policy>\r\n<site-control permitted-cross-domain-policies=\"master-only\"/>\r\n<allow-access-from domain=\"*\" to-ports=\"*\" />\r\n</cross-domain-policy>\0";
        }

    }
}
