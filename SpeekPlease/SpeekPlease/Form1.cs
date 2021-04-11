using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Speech.Synthesis.TtsEngine;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;



namespace SpeekPlease
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string FielText
        {
            get { return richTextBox1.Text; }
            set { richTextBox1.Text = value; }
        }
        SpeechSynthesizer speechSynthesizerObj;
        bool mode = true;
        static SpeechRecognitionEngine engine;
        private void Form1_Load(object sender, EventArgs e)
        {
            speechSynthesizerObj = new SpeechSynthesizer();
            btnResume.Enabled = false;
            btnPause.Enabled = false;
            btnStop.Enabled = false;
            Console.WriteLine(richTextBox1.Text);
            richTextBox1.Clear();
        }
        static void engine_SpeechRecognized(object ob, SpeechRecognizedEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.richTextBox1.Text = e.Result.Text;
            Console.WriteLine(e.Result.Text);
            
        }
        static Grammar MyGrammar()
        {
            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;

            DictationGrammar spellingDictationGrammar =
                new DictationGrammar("grammar:dictation#spelling");
            spellingDictationGrammar.Name = "spelling dictation";
            spellingDictationGrammar.Enabled = true;

            DictationGrammar customDictationGrammar =
                new DictationGrammar("grammar:dictation");
            customDictationGrammar.Name = "question dictation";
            customDictationGrammar.Enabled = true;

            return defaultDictationGrammar;
        }
        private void btnSpeak_Click(object sender, EventArgs e)
        {
            if (mode == true)
            {
                speechSynthesizerObj.Dispose();
                if (richTextBox1.Text != "")
                {
                    speechSynthesizerObj = new SpeechSynthesizer();
                    speechSynthesizerObj.SpeakAsync(richTextBox1.Text);
                    btnPause.Enabled = true;
                    btnStop.Enabled = true;
                }
            }
            else
            {
                

                engine = new SpeechRecognitionEngine();
                engine.SetInputToDefaultAudioDevice();
                Grammar g = MyGrammar();
                engine.LoadGrammar(g);
                engine.RecognizeAsync(RecognizeMode.Multiple);
                engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(engine_SpeechRecognized);
                richTextBox1.Text = Console.ReadLine();
                
            }
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            if(speechSynthesizerObj != null)
            {
                if(speechSynthesizerObj.State == SynthesizerState.Paused)
                {
                    speechSynthesizerObj.Resume();
                    btnResume.Enabled = false;
                    btnSpeak.Enabled = true;
                }
            }
        }
  
        private void btnPause_Click(object sender, EventArgs e)
        {
            if(speechSynthesizerObj != null)
            {
                if(speechSynthesizerObj.State == SynthesizerState.Speaking)
                {
                    speechSynthesizerObj.Pause();
                    btnResume.Enabled = true;
                    btnSpeak.Enabled = false;
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if(speechSynthesizerObj != null)
            {
                speechSynthesizerObj.Dispose();
                btnSpeak.Enabled = true;
                btnResume.Enabled = false;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
            }
        }
        private void changeModToSpeakign()
        {
            label1.Text = "Current mode: Reading";
            btnSpeak.Text = "Read";
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Current mode: Speaking")
            {
                changeModToSpeakign();
                mode = true;
            }
            else
            {
                label1.Text = "Current mode: Speaking";
                btnSpeak.Text = "Speak";
                mode = false;
            }
        }
    }
}
