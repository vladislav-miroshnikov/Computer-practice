using System;

namespace ChatLibrary
{
    static class InteractionProtocol
    {
       public static string MessageProcessing(string input)
       {
            input = input.Remove(0, 1);
            try
            {
                if (input.Length > 0)
                {
                    char firstLetter = input[0];
                    string message = input.Remove(0, 1);

                    switch (firstLetter)
                    {
                        case '*':
                            {
                                return ("*" + FromFullNamesToIPs(message));
                            }
                        case '#':
                            {
                                return ("+" + FromFullNamesToIPs(message));
                            }
                        case '-':
                            {
                                return ("-" + FromFullNamesToIPs(message));
                            }
                        default:
                            {
                                input = input.Remove(0, input.LastIndexOf(':') + 2);
                                return (input);
                            }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return null;
            }
       }

        private static string FromFullNamesToIPs(string input)
        {
            string iPs = string.Empty;
            string[] fullNames = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var fullName in fullNames)
            {
                string ip = fullName.Substring(fullName.IndexOf('(') + 1, fullName.LastIndexOf(')') - fullName.IndexOf('(') - 1) + ",";
                iPs += ip;
            }

            return (iPs.Remove(iPs.Length - 1));
        }
    }
}
