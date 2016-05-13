using SocketEx;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PandaChat
{
    public class Chatting
    {
        //TCP Connection objects
        StreamWriter writer;
        TcpClient client;
        Stream stream;
        StreamReader reader;
        //Connection info, password in another class, channel taken from the constructor
        string server = "chat.freenode.net";
        int port = 6667;
        string name = "NoName";
        string channel = "#PandaChat";
        //seperate thread to maintain the connection and read from server
        Thread listening;
        //list of users in the channel
        public List<string> list_of_users = new List<string>();

        //the constructor
        public Chatting(string name)
        {
            this.name = name;
        }

        //tell a command to the server you're connected to
        public void tell_server(string tellwut)
        {
            writer.WriteLine(tellwut);
            writer.Flush();
        }

        //say something in chatroom you're in
        public void say(string saywut)
        {
            writer.WriteLine("PRIVMSG {0} :{1}", channel, saywut);
            writer.Flush();
        }

        //connect to the server and join the channel
        public void connect()
        {
            //Connect
            client = new TcpClient(server, port);
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            //Join the channel
            tell_server("PASS none");
            tell_server("USER " + name + " 0 * :" + name);
            tell_server("NICK " + name);
            tell_server("JOIN " + channel);
            //start the thread maintaining the connection
            listening = new Thread(maintain_connection);
            listening.Start();
        }

        //disconnect from the server
        public void disconnect()
        {
            listening.Abort();
            reader.Close();
            writer.Close();
            stream.Close();
            client.Dispose();
        }

        //checks who said the line
        private string who_says_that(string input)
        {
            return input.Substring(1, input.IndexOf("!") - 1);
        }

        //method for maintaining the connection and parsing what server tells us
        private void maintain_connection() //maintain the connection and parse the reader
        {
            string command = "";
            while ((command = reader.ReadLine()) != null)
            {
                if (command.StartsWith("PING :")) //replying to server pings
                {
                    tell_server("PONG :" + command.Substring(6));
                }
                if (command.Contains("JOIN #pandachat") || command.Contains("JOIN #PandaChat")) //connected to the chatroom
                {
                    App.chatPage.consolewrite("Connected!");
                }
                else if (command.ToLower().Contains("privmsg #pandachat :")) //message detected
                {
                    string nick = who_says_that(command);
                    if (command.Contains("PRIVMSG #pandachat :"))
                    {
                        string msg = command.Substring(command.IndexOf("PRIVMSG #pandachat :") + 20);
                        App.chatPage.consolewrite(nick + ": " + msg);
                    }
                    else if (command.Contains("PRIVMSG #PandaChat :"))
                    {
                        string msg = command.Substring(command.IndexOf("PRIVMSG #PandaChat :") + 20);
                        App.chatPage.consolewrite(nick + ": " + msg);
                    }
                }
                else if (command.Contains(" = #pandachat :") && command.Contains(" @ChanServ"))
                {
                    int from = command.IndexOf(" = #pandachat :") + " = #pandachat :".Length;
                    int to = command.IndexOf(" @ChanServ");
                    string users = command.Substring(from, to - from);
                    string[] table_of_users = users.Split(' ');
                    list_of_users.Clear();
                    foreach (string user in table_of_users)
                    {
                        list_of_users.Add(user);
                    }
                }
            }
        }
    }
}
