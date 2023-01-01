using System;
using System.Windows.Forms;
using org.mozilla.javascript;
using Tulpep.NotificationWindow;
using System.Collections.Generic;
using System.IO;
using System.Net;
using NSoup;
using Windows.UI.Notifications;

namespace RhinoApplication
{
    public partial class Form1 : Form
    {
       
        
        public Form1()
        {
            InitializeComponent();
            this.textBox1.AutoWordSelection = true;
            this.textBox1.AutoWordSelection = false;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            boolResult = true;
            consoles = false;
            cons = "";
            String result = run(textBox1.Text.ToString());
            if (boolResult || consoles)
                textBox2.Text = cons.Length == 0 ? result : cons;
            textBox1.Focus();
        }
        static string cons = "";
        static Boolean boolResult;

        static public void setBool()
        {
            boolResult = false;
        }

        Function response;
        Context MyContext;
        ScriptableObject MyScope;

        public void Initializes()
        {

            //System.GC.Collect();
            //System.GC.WaitForPendingFinalizers();
            Context rhino = Context.enter();
            MyContext = rhino;
            rhino.setLanguageVersion(Context.VERSION_ES6);
            rhino.setOptimizationLevel(-1);
           
            try
            {
                ScriptableObject top = new ImporterTopLevel(rhino);
                Scriptable scope = rhino.initStandardObjects(top);
                MyScope = (ScriptableObject)scope;
                ScriptableObject.putProperty(scope, "textBox", textBox2);
                ScriptableObject.putProperty(scope, "ctx", rhino);
                ScriptableObject.putProperty(scope, "mainClass", new Form1());
                ScriptableObject.putProperty(scope, "console", new console());
                //object sc = Context.javaToJS(new function(), scope);
                //scope.put("ccc", scope, sc);
                ((ScriptableObject)scope).defineFunctionProperties(new String[] {"prints", "print", "Jsoup" }, typeof(function), ScriptableObject.DONTENUM);
                java.lang.Class cls = typeof(Form1);
                java.lang.reflect.Member method = cls.getMethod("setBool");
                Scriptable fun = new FunctionObject("setResult", method, scope);
                scope.put("falseResult", scope, fun);
                Script str = rhino.compileString("function response(str){return eval(str)+'';}", "MyScript", 0, null);
                str.exec(rhino, scope);
                response = (Function)scope.get("response", (Scriptable)scope);
            }
            catch (Exception e)
            {
            }
            //Context.exit();

        }
        public string run(string str)
        {
            try
            {
                string addStr="";
                string res;
                if (File.Exists("this.js"))
                    addStr = File.ReadAllText("this.js") + "\r\n";
                Context.enter();
                res = Context.toString(response.call(MyContext, MyScope, MyScope, new Object[] { addStr + str }));
                //Context.exit();
                return res;
            }
            catch (Exception e)
            {
                return ""+e;
            }
        }

        public static bool consoles = false;

        public class console
        {
            public String log(String str)
            {
                if (!consoles) consoles = true;
                cons += str + "\r\n";
                return "";
            }
        }

        public static class function
        {
            public static void print(string str)
            {
                PopupNotifier popup = new PopupNotifier();
                //popup.Image
                popup.TitleText = "Print Window";
                popup.TitleFont = new System.Drawing.Font("Arial", 20);
                popup.TitlePadding = new Padding(85, 10, 0, 0);
                popup.ContentPadding = new Padding(15, 10, 0, 15);
                popup.Size = new System.Drawing.Size(350, 300);
                popup.ContentText = str + "\r\n";
                popup.ContentFont = new System.Drawing.Font("Arial", 13);
                popup.HeaderColor = System.Drawing.Color.White;
                popup.TitleColor = System.Drawing.Color.Black;
                popup.ContentHoverColor = System.Drawing.Color.Black;
                popup.ContentColor = System.Drawing.Color.Gray;
                popup.ButtonHoverColor = System.Drawing.Color.White;
                popup.ShowGrip = false;
                popup.HeaderHeight = 1;
                popup.Popup();
            }
            public static void prints(string str)
            {
                var xml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                var text = xml.GetElementsByTagName("text");
                text[0].AppendChild(xml.CreateTextNode(str));
                var toast = new ToastNotification(xml);
                ToastNotificationManager.CreateToastNotifier("RhinoJS").Show(toast);
            }
            public static NSoup.Nodes.Document Jsoup(String url)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            return NSoupClient.Parse(reader.ReadToEnd());
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


        private void textbox1_keyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                button1_Click(sender, e);
                SendKeys.Send("{ESC}");
            }
            if (e.KeyCode == Keys.F12)
            {
                if (this.WindowState == FormWindowState.Maximized)
                    WindowState = FormWindowState.Normal;
                else
                    this.WindowState = FormWindowState.Maximized;
            }
            if(e.KeyCode == Keys.F3)
            {
                new Form2().Show();
            }
            if(e.Control && e.KeyCode == Keys.N)
                new Form1().Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initializes();
        }
    }

}
