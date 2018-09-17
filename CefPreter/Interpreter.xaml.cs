using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using CefSharp;
using CefSharp.Wpf;

namespace CefPreter
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Interpreter : UserControl
    {
        List<Expression> Expressions;
        Browser.Browser Browser { get; set; }

        CefMemory Memory;
        public Interpreter()
        {
            var cache = System.IO.Path.Combine(Environment.CurrentDirectory, System.IO.Path.Combine("cache"));
            if (!System.IO.Directory.Exists(cache))
                System.IO.Directory.CreateDirectory(cache);
            
            var settings = new CefSettings() { CachePath = cache };
            Cef.Initialize(settings);

            InitializeComponent();

            Browser = new Browser.Browser();
            Browser.Chromium = Chromium;

            Processed = true;
            Completed += Stop;
        }

        private List<Token> LexicalAnalize()
        {
            Lexer lexer = new Lexer(Code);
            return lexer.tokens;
        }

        private List<Expression> ExpressionsParse(List<Token> tokens)
        {
            Parser parser = new Parser();
            var expressions = parser.ParseExpressions(tokens);
            return expressions;
        }

        public void Analize()
        {
            try
            {
                var tokens = LexicalAnalize();
                this.Expressions = ExpressionsParse(tokens);

            }
            catch (Exception ex)
            {
                Output += ex.Message + " in " + ex.StackTrace;
            }
        }

        public async Task<bool> Run()
        {
            Output = "";
            MemoryDump = "";
            Processed = false;
            ExpressionResult exres = ExpressionResult.OK;
            try
            {
                this.Analize();
                Memory = new CefMemory();
                for (var i = 0; i < Expressions.Count; i++)
                {
                    
                    if (Processed) return false;
                    if (exres == ExpressionResult.CondFalse)
                    {
                        exres = ExpressionResult.OK;
                    }
                    else
                    {
                        Memory.Update(Expressions[i].RequiredMemory());
                        exres = await Expressions[i].Execute(Browser, Memory, Callback, Log);//expression writes variables to the memory!!
                                                                              //memoryScope.Update(expression.Memory);//updates all values
                    }


                    
                }
            }
            catch (Exception ex)
            {
                Output += ex.Message + " in " + ex.StackTrace;
            }

            

            Completed();
            return true;
        }


        public void Log()
        {
            string str = "";
            try { str += ((global::CefPreter.Types.String)Memory.Get("Print")).Value; }
            catch {
                //vse norm, tak tozhe mozhna, reli
            }
            if (!String.IsNullOrWhiteSpace(str))
            {
                
                this.Output += "\r\n" + str;
                Memory.Set(Types.Variable.Create("Print", ""));
            }
            MemoryDump = Memory.ToString();
        }

        public string MemoryDump
        {
            get { return (string)GetValue(MemoryDumpProperty); }
            set { SetValue(MemoryDumpProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MemoryDump.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MemoryDumpProperty =
            DependencyProperty.Register("MemoryDump", typeof(string), typeof(Interpreter), new PropertyMetadata(""));



        public string Output
        {
            get { return (string)GetValue(OutputProperty); }
            set { SetValue(OutputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Output.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputProperty =
            DependencyProperty.Register("Output", typeof(string), typeof(Interpreter), new PropertyMetadata(""));



        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Code.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register("Code", typeof(string), typeof(Interpreter), new PropertyMetadata(""));

        public delegate void InterpreterEventHandler();

        public delegate void CallBackEventHandler(CefMemory memory);

        public event InterpreterEventHandler Completed;
        public event InterpreterEventHandler Stopped;

        public event CallBackEventHandler Callback;



        public bool Processed
        {
            get { return (bool)GetValue(RunningProperty); }
            set { SetValue(RunningProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Running.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RunningProperty =
            DependencyProperty.Register("Running", typeof(bool), typeof(Interpreter), new PropertyMetadata(false));


        
        public void Stop()
        {
            Processed = true;
            Stopped();
        }
    }

    class WrongParamsCountException : Exception
    {

    }

    class UnknownFunction : Exception
    {

    }




}