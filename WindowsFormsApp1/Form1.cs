using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public static ChromiumWebBrowser chromeBrowser;
        public CallbackObjectForJs CallbackObjectForJs_ex;
        public Form1()
        {
            InitializeComponent();

            InitializeChromium();

            int formHeight = this.Height;
            int formWidth = this.Width;
            chromeBrowser.Size = new Size(formWidth - 10, formHeight - 20);
            chromeBrowser.ClientSize = new Size(formWidth - 10, formHeight - 40);
            chromeBrowser.SendToBack();
        }

        public void InitializeChromium()
        {
            try
            {
                CefSettings settings = new CefSharp.WinForms.CefSettings
                {
                    RemoteDebuggingPort = 8088,
                    LogFile = System.AppDomain.CurrentDomain.BaseDirectory + "CefLog.log", //You can customise this path
                    LogSeverity = LogSeverity.Default
                };


                settings.LogFile = System.AppDomain.CurrentDomain.BaseDirectory + "CefLog.log";
                settings.LogSeverity = LogSeverity.Default;
                settings.RemoteDebuggingPort = 8088;
                // settings.CachePath = "cache_path112";
                settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";
                ///settings.CefCommandLineArgs.Add("disable-gpu");
                ///  settings.CefCommandLineArgs.Add("disable-gpu-shader-disk-cache", "1");
                //settings.CefCommandLineArgs.Add("disable-gpu-compositing");
                // Initialize cef with the provided settings


                settings.RegisterScheme(new CefCustomScheme
                {
                    SchemeName = "localfolder",
                    DomainName = "testlauncher",
                    SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                rootFolder: AppDomain.CurrentDomain.BaseDirectory + "\\html",
                hostName: "testlauncher",
                defaultPage: "index.html" // will default to index.html
            )
                });

                CallbackObjectForJs_ex = new CallbackObjectForJs();

                //  settings.CefCommandLineArgs.Add("disable-gpu"); // Disable GPU acceleration
                /// settings.CefCommandLineArgs.Add("disable-gpu-vsync"); //Disable GPU vsync
                Cef.EnableHighDPISupport();
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

                //  System.Diagnostics.Process.Start("netsh advfirewall firewall add rule name =\"Valhlalla Launcher\" dir =in action = allow program = \"" + AppDomain.CurrentDomain.BaseDirectory + "\\cef_example.exe\" enable = yes");
                //  System.Diagnostics.Process.Start("netsh advfirewall firewall add rule name =\"Valhlalla Launcher Bug Reporter\" dir =in action = allow program = \"" + AppDomain.CurrentDomain.BaseDirectory + "\\BugReporter\\BugReporter.exe\" enable = yes");
                //  System.Diagnostics.Process.Start("netsh advfirewall firewall add rule name =\"Valhlalla Launcher CefSubProccess_x86\" dir =in action = allow program = \"" + AppDomain.CurrentDomain.BaseDirectory + "\\x86\\CefSharp.BrowserSubprocess.exe\" enable = yes");
                //  System.Diagnostics.Process.Start("netsh advfirewall firewall add rule name =\"Valhlalla Launcher CefSubProccess_x64\" dir =in action = allow program = \"" + AppDomain.CurrentDomain.BaseDirectory + "\\x64\\CefSharp.BrowserSubprocess.exe\" enable = yes");

                // Create a browser component
               
                try
                {
                    chromeBrowser = new ChromiumWebBrowser("localfolder://testlauncher/index.html");
                }
                catch (Exception ex)
                {
                    string ggg = "";

                   
                }

                /// chromeBrowser.CanExecuteJavascriptInMainFrame = true;

                chromeBrowser.JavascriptObjectRepository.Register("callbackObj", CallbackObjectForJs_ex, isAsync: false);

                // Add it to the form and fill it to the form window.
                chromeBrowser.Location = new Point(1, 1);



                this.Controls.Add(chromeBrowser);
                chromeBrowser.Dock = DockStyle.None;
                chromeBrowser.SendToBack();
                
              //  panel1.BringToFront();
               // panel1.Show();

               /// timer_recheck.Enabled = true;
            }
            catch (Exception ex)
            {
               ///
            }

          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        public class CallbackObjectForJs
        {


            public static string request_to_minimize = "";
            public static int prev_width = 0;
            public static int prev_height = 0;
            public static string current_window_state = "";

            public string move_status_disk1 = "";
            public double move_percent_disk1 = 0;
            public string stop_move_disk1 = "";

            public string move_status_disk2 = "";
            public double move_percent_disk2 = 0;
            public string stop_move_disk2 = "";

            public struct device
            {
                public string name;
            }

            public struct transfer_history
            {
                public string id;
                public string date;
                public string time;
                public string job;
                public string device;
                public string size;
                public string transfer_status;
            }

            public struct history_answer
            {
                public List<transfer_history> all_history;
            }

            public struct directory_in_path
            {
                public string name;
                public long size;
            }

            public struct device_answer
            {
                public List<device> all_devices;
            }

            public struct disk_info_answer
            {
                public string status_disk1;
                public long size_disk1;
                public string disk_1_real_path;
                public string status_disk2;
                public long size_disk2;
                public string disk_2_real_path;
            }

            public struct move_status_answer
            {
                public string move_status_disk1;
                public double move_percent_disk1;
                public string move_status_disk2;
                public double move_percent_disk2;
            }

            public struct directory_in_path_answer
            {
                public List<directory_in_path> all_directories;
            }


            public string getVersion()
            {
                return "firsttestVersion";
            }


            public string setUserSettings(string key, string value)
            {
                try
                {
                    RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\FileCopyManager");
                    registryKey.SetValue(key, value);
                    registryKey.Close();
                    return value;
                }
                catch (Exception exx)
                {

                    return "null";
                }
            }

            public int getTotalCountofFiles(string dir)
            {
                var files = Directory.EnumerateFiles(dir + "\\", "*", SearchOption.AllDirectories);
                return files.Count();
            }

            public string getMoveStatus()
            {
                move_status_answer move_status_answer = new move_status_answer();
                move_status_answer.move_status_disk1 = move_status_disk1;
                move_status_answer.move_percent_disk1 = move_percent_disk1;
                move_status_answer.move_status_disk2 = move_status_disk2;
                move_status_answer.move_percent_disk2 = move_percent_disk2;

                var json_result_def = JObject.FromObject(move_status_answer).ToString(Formatting.None);

                return json_result_def.ToString();
            }

            public void stopMoveDisk1()
            {
                stop_move_disk1 = "stop";
            }

            public void stopMoveDisk2()
            {
                stop_move_disk2 = "stop";
            }


            public void startMoveDisk1(string source, string target)
            {
                
                Thread t1 = new Thread(
                             () =>
                             {

                                 move_status_disk1 = "moving";

                                 ////получение полного числа файлов
                                 int total_count_of_files = getTotalCountofFiles(source);
                                 ///создать директорию по девайс, если еще не существует

                                 if (!Directory.Exists(target))
                                 {
                                     Directory.CreateDirectory(target);
                                 }

                                 int current_file = 0;

                                 move_percent_disk1 = 0;

                                 var sourcePath = source.TrimEnd('\\', ' ');
                                 var targetPath = target.TrimEnd('\\', ' ');
                                 var files = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories)
                                                      .GroupBy(s => Path.GetDirectoryName(s));



                                 
                foreach (var folder in files)
                {

                                     if (stop_move_disk1 != "")
                                     {
                                         break;
                                     }

                                     var targetFolder = folder.Key.Replace(sourcePath, targetPath);
                    Directory.CreateDirectory(targetFolder);
                    foreach (var file in folder)
                    {
                                         /////обработка команды остановки
                                         if (stop_move_disk1 != "")
                                         {
                                             break;
                                         }

                                         var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));
                        if (File.Exists(targetFile)) File.Delete(targetFile);
                        File.Copy(file, targetFile);
                        current_file = current_file + 1;

                                         double percent_progress = Math.Round((double)(current_file * 100) / total_count_of_files, 2);


                                         move_percent_disk1 = percent_progress;
                        
                        ////chromeBrowser.ExecuteScriptAsync(callback_function + "('{\"status\":\"moving\",\"file\":\"" + Path.GetFileName(file) + "\"}')");
                    }
                }
                                 if (stop_move_disk1 != "")
                                 {
                                     stop_move_disk1 = "";
                                     move_status_disk1 = "";
                                     move_percent_disk1 = 0;
                                 }
                                 else
                                 {

                                     move_status_disk1 = "done";
                                     move_percent_disk1 = 100;
                                 }
                 });

                t1.Start();
                // deleteFolderSpecTemp(source, callback_function);
                // Directory.Delete(source, true);
            }


            public void startMoveDisk2(string source, string target)
            {

                Thread t1 = new Thread(
                             () =>
                             {

                                 move_status_disk2 = "moving";

                                 ////получение полного числа файлов
                                 int total_count_of_files = getTotalCountofFiles(source);
                                 ///создать директорию по девайс, если еще не существует

                                 if (!Directory.Exists(target))
                                 {
                                     Directory.CreateDirectory(target);
                                 }

                                 int current_file = 0;

                                 move_percent_disk2 = 0;

                                 var sourcePath = source.TrimEnd('\\', ' ');
                                 var targetPath = target.TrimEnd('\\', ' ');
                                 var files = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories)
                                                      .GroupBy(s => Path.GetDirectoryName(s));




                                 foreach (var folder in files)
                                 {

                                     if (stop_move_disk2 != "")
                                     {
                                         break;
                                     }

                                     var targetFolder = folder.Key.Replace(sourcePath, targetPath);
                                     Directory.CreateDirectory(targetFolder);
                                     foreach (var file in folder)
                                     {
                                         /////обработка команды остановки
                                         if (stop_move_disk2 != "")
                                         {
                                             break;
                                         }

                                         var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));
                                         if (File.Exists(targetFile)) File.Delete(targetFile);
                                         File.Copy(file, targetFile);
                                         current_file = current_file + 1;

                                         double percent_progress = Math.Round((double)(current_file * 100) / total_count_of_files, 2);


                                         move_percent_disk2 = percent_progress;

                                         ////chromeBrowser.ExecuteScriptAsync(callback_function + "('{\"status\":\"moving\",\"file\":\"" + Path.GetFileName(file) + "\"}')");
                                     }
                                 }
                                 if (stop_move_disk2 != "")
                                 {
                                     stop_move_disk2 = "";
                                     move_status_disk2 = "";
                                     move_percent_disk2 = 0;
                                 }
                                 else
                                 {

                                     move_status_disk2 = "done";
                                     move_percent_disk2 = 100;
                                 }
                             });

                t1.Start();
                // deleteFolderSpecTemp(source, callback_function);
                // Directory.Delete(source, true);
            }


            public string getUserSettings(string key)
            {
                string value = "null";

                try
                {
                    RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\FileCopyManager");
                    value = registryKey.GetValue(key).ToString();
                    registryKey.Close();
                }
                catch (Exception exx)
                {
                    /// LogToFile("не можем получить настройки " + key + ": " + exx.Message);
                }


                return value;
            }


            public string getRootPath()
            {
                ////читаем рут директорию из реестра
                string root_path = getUserSettings("root_path");
                if(!Directory.Exists(root_path))
                {
                    setRootPath("");
                    return "";
                } else
                {
                    return root_path;
                }
                
            }

            public string setRootPath(string path)
            {
                ////читаем рут директорию из реестра
                string root_path = setUserSettings("root_path", path);
                return root_path;
            }

            public string selectRootPath()
            {

                try
                {

                    /////открываем диалоговое окно выбора папки
                    string selectedPath = "";

                    var t = new Thread((ThreadStart)(() =>
                    {
                        FolderBrowserDialog fbd = new FolderBrowserDialog();
                        fbd.RootFolder = System.Environment.SpecialFolder.MyComputer;
                        fbd.ShowNewFolderButton = true;
                        if (fbd.ShowDialog(new Form() { TopMost = true }) == DialogResult.Cancel)
                            return;
                        selectedPath = fbd.SelectedPath;
                    }));
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                    t.Join();

                    setRootPath(selectedPath);
                    return selectedPath;
                }
                catch (Exception ex)
                {
                    /// LogToFile("команда на открытие диалога папка " + ex.Message);
                    return "";
                }
            }

            public string getListOfDevices()
            {

                device_answer device_answer = new device_answer();


                devices_database devices_database = new devices_database();
                try
                {
                    devices_database devices_db = new devices_database();

                    bool is_exist = false; ;

                    ///существует ли такой же контакт

                    SQLiteDataReader myReader = devices_db.select_query("SELECT * FROM devices");
                    List<device> all_devices = new List<device>();

                    while (myReader.Read())
                    {
                        string name = myReader["name"].ToString();

                        device new1 = new device();
                        new1.name = name;
                        all_devices.Add(new1);
                    }

                    device_answer.all_devices = all_devices;

                    var json_result_def = JObject.FromObject(device_answer).ToString(Formatting.None);

                    return json_result_def.ToString();
                }
                catch (Exception ex)
                {
                    return "{}";
                }

               
            }


            public string getHistory()
            {

                history_answer history_answer = new history_answer();

                try
                {
                    transfer_history_db transfer_history_db = new transfer_history_db();

      
                    ///существует ли такой же контакт

                    SQLiteDataReader myReader = transfer_history_db.select_query("SELECT * FROM transfer_history");
                    List<transfer_history> all_history = new List<transfer_history>();

                    while (myReader.Read())
                    {
                        string id = myReader["id"].ToString();
                        string date = myReader["date"].ToString();
                        string time = myReader["time"].ToString();
                        string job = myReader["job"].ToString();
                        string device = myReader["device"].ToString();
                        string size = myReader["size"].ToString();
                        string transfer_status = myReader["transfer_status"].ToString();

                        transfer_history new1 = new transfer_history();
                        new1.id = id;
                        new1.date = date;
                        new1.time = time;
                        new1.job = job;
                        new1.device = device;
                        new1.size = size;
                        new1.transfer_status = transfer_status;
                        all_history.Add(new1);
                    }

                    history_answer.all_history = all_history;

                    var json_result_def = JObject.FromObject(history_answer).ToString(Formatting.None);

                    return json_result_def.ToString();
                }
                catch (Exception ex)
                {
                    return "{}";
                }


            }


            public string getDirectoriesOfRootPath(string dir)
            {
                if (Directory.Exists(dir))
                {
                    try
                    {
                        string directory = "";
                        directory_in_path_answer directory_in_path_answer = new directory_in_path_answer();
                        List<directory_in_path> all_directories = new List<directory_in_path>();


                        foreach (string d in Directory.GetDirectories(dir))
                        {
                            directory = Path.GetFileName(d);
                            ///тут будем узнавать размер директории
                            var files = Directory.EnumerateFiles(dir + "\\" + directory + "\\", "*", SearchOption.AllDirectories);
                            long size = (from file in files let fileInfo = new FileInfo(file) select fileInfo.Length).Sum();

                            directory_in_path directory_in_path = new directory_in_path();
                            directory_in_path.name = directory;
                            directory_in_path.size = size;
                            all_directories.Add(directory_in_path);
                        }


                        directory_in_path_answer.all_directories = all_directories;
                        var json_result_def = JObject.FromObject(directory_in_path_answer).ToString(Formatting.None);

                        return json_result_def.ToString();
                    }
                    catch (Exception ex)
                    {
                        return "{}";
                    }
                } else
                {
                    setRootPath("");
                    return "{}";
                }
            }

            public string addNewDevice(string device_name)
            {
                try
                {
                    devices_database device_db = new devices_database();

                    bool is_exist = false; ;

                    ///существует ли такой же контакт

                    SQLiteDataReader myReader = device_db.select_query("SELECT * FROM devices");

                    while (myReader.Read())
                    {
                        if (myReader["name"].Equals(device_name))
                        {
                            is_exist = true;
                        }
                    }


                    if (is_exist == false)
                    {

                        string sql = "insert into devices (name) values('" +device_name + "')";
                        device_db.make_query(sql);
                        device_db.database_close();

                        return "added";
                    }
                    else
                    {

                        //string sql = "update game_accounts set count=count+1 where game_account='" + account + "'";
                        // game_database.make_query(sql);
                        device_db.database_close();
                        return "already_exists";
                    }

                }
                catch (Exception ex)
                {
                    string exxxx = "";
                    return "error";
                }

                
            }


            public string addNewTansferLog(string job, string device, string size, string transfer_status)
            {
                Thread t1 = new Thread(
                           () =>
                           {

                               try
                               {
                                   transfer_history_db device_db = new transfer_history_db();

                                   bool is_exist = false; ;


                                   SQLiteDataReader myReader = device_db.select_query("SELECT id FROM transfer_history");

                                   int id = 1;

                                   while (myReader.Read())
                                   {
                                       id = int.Parse(myReader["id"].ToString());
                                   }

                                   id = id + 1;
                                   DateTime now = DateTime.Now;
                                   string date = now.Month + "/" + now.Day + "/" + now.Year;
                                   string time = now.Hour + ":" + now.Minute;

                                   string sql = "insert into transfer_history (id, date, time, job, device, size, transfer_status) values('" + id.ToString() + "','" + date + "','" + time + "','" + job + "','" + device + "','" + size + "','" + transfer_status + "')";
                                   device_db.make_query(sql);
                                   device_db.database_close();

                                 
                               }
                               catch (Exception ex)
                               {
                                   string exxxx = "";
                                  
                               }
                           });

                t1.Start();
                return "added";

            }

            public string deleteDevice(string device_name)
            {
                try
                {
                    devices_database device_db = new devices_database();

                    bool is_exist = false; ;

                    ///существует ли такой же контакт

                    SQLiteDataReader myReader = device_db.select_query("SELECT * FROM devices");

                    while (myReader.Read())
                    {
                        if (myReader["name"].Equals(device_name))
                        {
                            is_exist = true;
                        }
                    }


                    if (is_exist == false)
                    {

                       
                        device_db.database_close();

                        return "not_found";
                    }
                    else
                    {

                        string sql = "delete from devices where name='" + device_name + "'";
                        device_db.make_query(sql);
                        device_db.database_close();
                        return "deleted";
                    }

                }
                catch (Exception ex)
                {
                    string exxxx = "";
                    return "error";
                }


            }

            public string getDiscStatus()
            {
                try
                {
                    disk_info_answer disk_info_answer = new disk_info_answer();
                    ///получаем информацию, какие диски проверяем
                    string disk1folder = getUserSettings("disk1");
                    string disk2folder = getUserSettings("disk2");

                    if ((disk1folder == "")||(disk1folder == "null"))
                    {
                        setUserSettings("disk1", "D:\\");
                        disk1folder = "D:\\";
                    }
                    if ((disk2folder == "")|| (disk2folder == "null"))
                    {
                        setUserSettings("disk2", "E:\\");
                        disk2folder = "E:\\";
                    }

                    ////проверяем, активен ли диск
                    if (Directory.Exists(disk1folder))
                    {
                        disk_info_answer.status_disk1 = "active";
                        ///считаем размер
                        var files = Directory.EnumerateFiles(disk1folder, "*", SearchOption.AllDirectories);
                        long size = (from file in files let fileInfo = new FileInfo(file) select fileInfo.Length).Sum();
                        disk_info_answer.size_disk1 = size;
                        disk_info_answer.disk_1_real_path = disk1folder;

                    }
                    else
                    {
                        disk_info_answer.status_disk1 = "inactive";
                        disk_info_answer.size_disk1 = 0;
                        move_status_disk1 = "";
                    }

                    if (Directory.Exists(disk2folder))
                    {
                        disk_info_answer.status_disk2 = "active";
                        ///считаем размер
                        var files = Directory.EnumerateFiles(disk2folder, "*", SearchOption.AllDirectories);
                        long size = (from file in files let fileInfo = new FileInfo(file) select fileInfo.Length).Sum();
                        disk_info_answer.size_disk2 = size;
                        disk_info_answer.disk_2_real_path = disk2folder;

                    }
                    else
                    {
                        disk_info_answer.status_disk2 = "inactive";
                        disk_info_answer.size_disk2 = 0;
                        move_status_disk2 = "";
                    }


                    var json_result_def = JObject.FromObject(disk_info_answer).ToString(Formatting.None);

                    return json_result_def.ToString();
                } catch(Exception ex)
                {
                    return "{}";
                }

            }

            public void deleteFolderSpecTemp(string folder, string first_level)
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(folder);
                    DirectoryInfo[] diA = di.GetDirectories();
                    FileInfo[] fi = di.GetFiles();
                    foreach (FileInfo f in fi)
                    {

                        f.Delete();
                       
                        

                    }
                    foreach (DirectoryInfo df in diA)
                    {
                        deleteFolderSpecTemp(df.FullName, "");
                        try
                        {

                            Directory.Delete(df.FullName, true);

                        }
                        catch (Exception ex)
                        {
                           /// LogToFile("Удаление временных файлов2 " + ex.Message);
                        }
                    }



                    if (di.GetDirectories().Length == 0 && di.GetFiles().Length == 0)
                    {

                        if(first_level=="")
                         di.Delete();

                    }
                }
                catch (Exception ex)
                {
                    string l = "1";
                    ////все удалилось
                    
                }



            }


            public string formatDisk1()
            {
                string disk1folder = getUserSettings("disk1");
               

                if ((disk1folder == "") || (disk1folder == "null"))
                {
                    setUserSettings("disk1", "D:\\");
                    disk1folder = "D:\\";
                }


                deleteFolderSpecTemp(disk1folder,"1");
                move_status_disk1 = "";

                return "formatted";
            }

            public string formatDisk2()
            {

                string disk2folder = getUserSettings("disk2");


                if ((disk2folder == "") || (disk2folder == "null"))
                {
                    setUserSettings("disk2", "E:\\");
                    disk2folder = "E:\\";
                }

                deleteFolderSpecTemp(disk2folder, "1");
                move_status_disk2 = "";

                return "formatted";

            }

            public void setFinish()
            {
                move_status_disk1 = "";
                move_status_disk2 = "";
            }

            public string createDirectory(string directory)
            {
                try
                {
                    Directory.CreateDirectory(getRootPath() + "\\" + directory);
                    if(Directory.Exists(getRootPath() + "\\" + directory))
                    {
                        return "created";
                    } else
                    {
                        return "error";
                    }

                } catch(Exception ex)
                {
                    return "error";
                }
            }


            public void setSelectedFolder(string selectedFolder)
            {
                setUserSettings("selected_folder", selectedFolder);

            }

            public string getSelectedFolder()
            {
                return getUserSettings("selected_folder");
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
            try
            {
                int formHeight = this.Height;
                int formWidth = this.Width;
                chromeBrowser.Size = new Size(formWidth - 10, formHeight - 20);
                chromeBrowser.ClientSize = new Size(formWidth - 10, formHeight - 40);
                chromeBrowser.SendToBack();
            }
            catch (Exception exx)
            {
                string eee = "";
            }
           
           
        }
    }
}
