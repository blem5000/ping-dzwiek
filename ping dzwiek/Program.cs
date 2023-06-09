using System;
using System.Net.NetworkInformation;
using System.Threading;

class Program
{
    static void Main()
    {
        int defaultValue = 3;
        string adres = null;
        string prawidlowy = null;
        string nieprawidlowy = null;
        bool validInput = false;
        string TestCoIle = null;
        int czas, odpowiedz;
        int odpowiedzdefault = 1000;

        Console.WriteLine("                                   *********************************************************");
        Console.WriteLine("                                   *                                                       *");
        Console.WriteLine("                                   *    Ping - konsola wydaje dzwiek gdy host odpowiada    *");
        Console.WriteLine("                                   *                                                       *");
        Console.WriteLine("                                   *********************************************************");

        while (string.IsNullOrWhiteSpace(adres))
        {
            Console.Write("Proszę wpisać IP bądź nazwę hosta: ");
            adres = Console.ReadLine();
        }

        do
        {
            Console.Write("Wpisz co ile należy przeprowadzić test (domyślnie: " + defaultValue + " sekundy): ");
            TestCoIle = Console.ReadLine();

            validInput = false;

            if (int.TryParse(TestCoIle, out czas))
            {
                validInput = true;
            }
            if (string.IsNullOrWhiteSpace(TestCoIle))
            {
                czas = defaultValue;
                validInput = true;
            }
            if(!validInput)
                Console.Write("Wybór nieprawidłowy. Proszę wpisać cyfrę lub nacisnąć enter by wybrać opcję domyślną\n");
        } while (!validInput);

        validInput = false;

        do
        {
            Console.Write("Wpisz jaki powinien być timeout odpowiedzi w ms (domyślnie: " + odpowiedzdefault + " milisekund): ");
            string odpowiedztekst = Console.ReadLine();

            validInput = false;

            if (int.TryParse(odpowiedztekst, out odpowiedz))
            {
                validInput = true;
            }
            if (string.IsNullOrWhiteSpace(odpowiedztekst))
            {
                odpowiedz = odpowiedzdefault;
                validInput = true;
            }
            if (!validInput)
                Console.Write("Wybór nieprawidłowy. Proszę wpisać cyfrę lub nacisnąć enter by wybrać opcję domyślną\n");
        } while (!validInput);

        validInput = false;

        do
        {
            Console.Write("Czy konsola ma sygnalizować dźwiękiem gdy host odpowie na ping (domyślnie tak)? (T/N): ");
            prawidlowy = Console.ReadLine();
            prawidlowy = prawidlowy.ToLower();

            if (prawidlowy.ToLower() == "t" || prawidlowy.ToLower() == "n")
            {
                validInput = true;
            }

            else if (string.IsNullOrWhiteSpace(prawidlowy))
            {
                prawidlowy = "t";
                validInput = true;
            }
            else
            {
                Console.Write("Wybór nieprawidłowy. Proszę nacisnąć literkę T,N lub nacisnąć enter by wybrać opcję domyślną");
            }

        } while (!validInput);

        validInput = false;

        do
        {
            Console.Write("Czy konsola ma sygnalizować dźwiękiem gdy host NIE odpowie na ping (domyślnie nie)? (T/N): ");
            nieprawidlowy = Console.ReadLine();
            nieprawidlowy = nieprawidlowy.ToLower();

            if (nieprawidlowy.ToLower() == "t" || nieprawidlowy.ToLower() == "n")
            {
                validInput = true;
                //nieprawidlowy = nieprawidlowy.ToLower();
            }

            else if (string.IsNullOrWhiteSpace(nieprawidlowy))
            {
                nieprawidlowy = "n";
                validInput = true;
            }
            else
            {
                Console.Write("Wybór nieprawidłowy. Proszę nacisnąć literkę T,N lub nacisnąć enter by wybrać opcję domyślną");
            }

        } while (!validInput);

        Console.Clear();

        Console.WriteLine("Testuje hosta: " + adres + " co: " + czas + " sekundy " + "z maksymalnym czasem odpowiedzi wynoszacym: " + odpowiedz);
        if (prawidlowy == "t")
            Console.WriteLine("Dźwiek jest włączony gdy test jest prawidlowy");
        else
            Console.WriteLine("Dźwiek jest wyłączony gdy test jest prawidlowy");

        if (nieprawidlowy == "t")
            Console.WriteLine("Dźwiek jest włączony gdy test jest nieprawidlowy");
        else
            Console.WriteLine("Dźwiek jest wyłączony gdy test jest nieprawidlowy");

        Console.WriteLine("By zakończyć test należy nacisnąć enter.\n");
        

        while (true)
        {
            bool pingResult = false;
            Ping ping = new Ping();
            try
            {
                PingReply reply = ping.Send(adres, odpowiedz); // Adjust timeout as needed

                if (reply != null && reply.Status == IPStatus.Success)
                {
                    pingResult = true;

                    if (prawidlowy == "t")
                        Console.Beep(880, 400);
                }
                else
                {

                    if (nieprawidlowy == "t")
                        Console.Beep(440, 400);
                }
            }
            catch (PingException ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write("Host nie został znaleziony, proszę podać poprawnego hosta: ");
                adres = Console.ReadLine();

            }

            if (pingResult)
            {
                PingReply reply = ping.Send(adres, odpowiedz);
                Console.WriteLine($"Ping zwrócony w czasie: {reply.RoundtripTime}ms");
            }
            else
            {
                Console.WriteLine($"Ping nie został zwrócony w ustalonym czasie {odpowiedz} ms");
            }
            Thread.Sleep(czas * 1000);
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                break;
        }
    }
}
