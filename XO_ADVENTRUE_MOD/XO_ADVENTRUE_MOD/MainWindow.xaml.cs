using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XO_ADVENTRUE_MOD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static System.Media.SoundPlayer sound_play = new System.Media.SoundPlayer();
        public static string FP_combat = "";
        public static int amount = 0;
        public static string FP_game = "";
        public static string FP_chat = "";
        public static SpeechSynthesizer speech;
        public static int work = 1;  //1 инициализация  /2 есть процесс /-1 пора спать
        public MainWindow()
        {

            InitializeComponent();
            main = this;
            this.Title = "xo пробуждение мод v 0.1.2b";
            label1.Content = "Поиск процесса crossout";

            Thread thread2 =
             new Thread(t =>
             {
                 MonitorCrossout();
             })
             { IsBackground = true };
            thread2.SetApartmentState(ApartmentState.STA);
            thread2.Start();
            Thread thread3 =
             new Thread(t =>
             {
                 Negotiate_logs();
             })
             { IsBackground = true };
            thread3.SetApartmentState(ApartmentState.STA);
            thread3.Start();

            Thread thread =
            new Thread(t =>
            {
                Thread_chat_probujdenie_read();
            })
            { IsBackground = true };
            thread.Start();
          
        }

        public static void MonitorCrossout() //мониторит кросыч
        {

            int d = 0;
            while (true)
            {
                d++;
                if ((System.Diagnostics.Process.GetProcessesByName("crossout").Length > 0))
                {
                    MainWindow.main.Status = "Процесс crossout обнаружен.";
                   
                    if(d>7)
                        Thread.Sleep(5000);
                    
                    if (work == 1)
                    {
                        work = 2;
                        Negotiate_logs();
                    }
                    work = 2;

                }
                else
                if (!(System.Diagnostics.Process.GetProcessesByName("crossout").Length > 0))
                {
                    MainWindow.main.Status = "Процесс crossout отключился. Поиск...";
                    // MainWindow.main.off = "Процесс crossout отключился. Поиск...";
                    work = 1;
                    if (Int32.TryParse(MainWindow.main.rtb_wpf, out int amount))
                    {
                        if (amount > 0)
                        {
                            MainWindow.main.rtb_wpf2 = "";
                        }
                    }
                    else
                    break;
                }
               
                Thread.Sleep(100);
            }
        }
        internal static MainWindow main;
        internal string Status
        {
            get { return label1.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { label1.Content = value; })); }
        }
        internal string Status2
        {
            get { return label2.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { label2.Content = value; })); }
        }
        internal string rtb_wpf
        {
            get { return listbox.Items.Count.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { listbox.Items.Add(value); })); }
        }
        internal string rtb_wpf2
        {
            get { return listbox.Items.Count.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { listbox.Items.Clear(); })); }
        }
        internal string off
        {
            get { return ""; }
            set { Dispatcher.Invoke(new Action(() => { try { Application.Current.Shutdown(); } catch { } })); }
        }
        public static void Negotiate_logs()
        {
            while (true)
            {
                if (work == 1)
                {

                }
                else
                    if (work == 2)
                {
                    break;
                }
                else
                {
                    return;
                }
                Thread.Sleep(500);
            }
            string highDir = "";//наша итоговая папочка с логами
                                //сам путь к логу аля shit.log
            string userName = Environment.UserName;  //юзернейм в винде
            string path = "C:\\Users\\" + userName + "\\Documents\\My Games\\Crossout\\logs\\";   //папка к логам, буем искать там самую свежую папку кста

            DateTime lastHigh = new DateTime(1900, 1, 1); // ебашим точку отсчета(дата)
            foreach (string subdir in Directory.GetDirectories(path))
            {
                DirectoryInfo fi1 = new DirectoryInfo(subdir);
                DateTime created = fi1.LastWriteTime;
                if (created > lastHigh)
                {
                    highDir = subdir;        //свежайшая папочка будет принята тутя 
                    lastHigh = created;
                }
            }
            FP_combat = highDir + "\\combat.log";  //адрес на текущий лог
            FP_game = highDir + "\\game.log";
            FP_chat = highDir + "\\chat.log";          // = "Live Logs {0}" + Combat_log_file_path;
            MainWindow.main.Status2 = FP_chat;
        }
        public static void Thread_chat_probujdenie_read()
        {
            while (true)
            {
                if (work == 1)
                {
                      
                }
                else
                    if (work == 2)
                {
                    break;
                }
                else
                {
                   
                }
                Thread.Sleep(500);
            }
            speech = new SpeechSynthesizer();
            while (true)
            {
                if (FP_chat == null || FP_chat == "")
                {
                    Thread.Sleep(500);
                }
                else { Thread.Sleep(500); break; }
            }
            var fileStream = new FileStream(FP_chat, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            string buffer = FP_chat;
            fileStream.Position = fileStream.Length;
            int rowNum = 0; //число считанных строк

            string line;  //считанная строка
            while (true)  //как грица - не закончил - сосешь хуй
            {

                if (buffer != FP_chat)
                {
                    fileStream = new FileStream(FP_chat, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    fileStream.Position = fileStream.Length;
                    rowNum = 0;
                }
                if (work == 2)
                {
                    
                    StreamReader streamReader = new StreamReader(fileStream);
                    while (streamReader.Peek() != -1)
                    {
                        rowNum++;
                        line = streamReader.ReadLine();
                        string output = String.Format(line);
                        if (!string.IsNullOrEmpty(output))
                        {
                            if (output.Contains("GAMEPLAY") && output.Contains("CHAT"))
                            {
                                string[] concoe = output.Split('>');
                                //       RichTextBox_1_thread(concoe[1]);
                                Make_sound(concoe[1]);
                                
                            }
                        }
                        if (work <= 1)
                            Thread.Sleep(500);
                        //11:22:20.854     CHAT| <		   GAMEPLAY> Ладно. Дело за малым — выкрасть у Мусорщиков фуру с электроникой.

                    }
                }
                else
                if (work <= 1)
                    Thread.Sleep(500);
                Thread.Sleep(10);

            }
        }

        public static void Make_sound(string what_to_audise)
        {

            string fullName_path = "";
            string way = Environment.CurrentDirectory;
            if (Directory.Exists(way + "\\adventure_sounds_audio"));
            {
                string[] readText = File.ReadAllLines(way + "\\adventure_sounds_audio" + "\\list_of_sounds.txt");
                foreach (string s in readText)
                {
                    if (s.Contains(what_to_audise))
                    {
                        if (s.Contains("comment:"))
                        {
                            continue;
                        }
                        if (s.Contains("void"))
                        {
                            MainWindow.main.rtb_wpf = "заглушено:" + what_to_audise;
                            return;
                        }
                        string[] concoe = s.Split('@');
                        //       RichTextBox_1_thread(concoe[1]);
                        if (concoe[1] != null)
                        {
                            fullName_path = way + "\\adventure_sounds_audio\\" + concoe[0];
                            break;
                        }
                    }
                }
            }
            //   DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path_sound_folder);
            //     FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + number_first_half + "*.*");

                

            if (fullName_path.Contains(".wav"))
            {
                MainWindow.main.rtb_wpf = "озвучено:" + what_to_audise;
                sound_play.SoundLocation = fullName_path;
                sound_play.Load();
                sound_play.Play(); // also soundPlayer.PlaySync()
                return;
            }
            else
            {
                if (!what_to_audise.Contains("#"))
                {
                    MainWindow.main.rtb_wpf = "отсутствует для:" + what_to_audise;
                    speech.Speak(what_to_audise);
                    return;
                }
                    
                else
                {
                    MainWindow.main.rtb_wpf ="катсцена:" +what_to_audise;
                    return;
                }
            }

            
                MainWindow.main.rtb_wpf = "пустышка?:" + what_to_audise;
            
        }

        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)(sender);
            var selected = lb.SelectedItem;
            
            if (selected != null)
            {
                string[] concoe = selected.ToString().Split(':');
                Clipboard.SetText(concoe[1]);
            }
                
        }
    }
}

