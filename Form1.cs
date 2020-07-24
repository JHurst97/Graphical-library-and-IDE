using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASEProject
{
    /// <summary>Driver class</summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class Form1 : Form
    {
        Graphics g;
        Pen myPen;
        private Point currLoc = new Point(0, 0);
        List<VarInt> intArr = new List<VarInt>();
        List<Method> methodArr = new List<Method>();
        ShapeFactory sf = new ShapeFactory();
        ArrayList shapes = new ArrayList();


        /// <summary>Initializes a new instance of the <see cref="Form1"/> class.</summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void editor_TextChanged(object sender, EventArgs e)
        {

        }

        private void commLine_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>Handles the KeyDown event of the commLine control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void commLine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String lastLine = commLine.Lines[commLine.Lines.Length - 1];

                Parser(lastLine);
            }
        }

        /// <summary>Handles the Click event of the runToolStripMenuItem1 control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBuilder mb = new MessageBuilder();
            String input = editor.Text;
            Parser(input);
        }

        /// <summary>Parsers the specified input.</summary>
        /// <param name="input_">The input.</param>
        private void Parser(String input_)
        {
            String loopContents = "";
            String methodContents = "";
            commLine.Text += "/";
            string input = input_;

            //LOOP
            bool hasLoop = input.Contains("loop");
            if (hasLoop)
            {
                loopContents = input.Split('{', '}')[1];
                //remove contents of loop as it is now saved in 'loopContents'
                input = input.Replace(loopContents, "");
            }

            //METHOD
            bool hasMethod = input.Contains("method");
            if (hasMethod)
            {
                methodContents = input.Split('{', '}')[1];
                //remove contents of method as it is now saved in 'methodContents'
                input = input.Replace(methodContents, "");
            }

            //each statement split with ';'
            string[] statements = input.Split(';');
            //remove final statement (';' error)
            statements = statements.Reverse().Skip(1).Reverse().ToArray();
            foreach (var statement in statements)
            {
                String current = statement;
                //remove line-break
                current = current.Replace("\r\n", "");
                //clear?
                if (current == "clear")
                {
                    panel1.Refresh();
                    currLoc.X = 0;
                    currLoc.Y = 0;
                }
                else if (current == "printIntArray")
                {
                    printIntArray();
                }
                else
                {
                    //outside curly brackets
                    String command = current.Split('(', ')')[0];
                    //inside curly brackets 
                    String parameters = current.Split('(', ')')[1];
                    String[] splitP = parameters.Split(',');

                    //compare each parameter to every element in the VarInt List
                    int count = 0;
                    foreach (string test in splitP)
                    {
                        for (int i = 0; i < intArr.Count; i++)
                        {
                            if (test == intArr[i].getName())
                            {
                                int value = intArr[i].getValue();
                                splitP[count] = value.ToString();
                            }
                        }
                        count++;
                    }

                    //search for methods

                    //string arr for names
                    string[] methodNames = new string[methodArr.Count];
                    for(int i = 0; i < methodArr.Count; i++)
                    {
                        methodNames[i] = methodArr[i].name;
                    }


                    int countM = 0;
                    foreach (string mName in methodNames)
                    {
                        String methodS = mName;
                        int resultsMethComp = String.Compare(command, methodS);

                        if (resultsMethComp == 0)
                        {
                            Console.WriteLine("hit!");
                            //string[] pAfterName = splitP.Skip(1).ToArray();
                            methodArr[countM].replaceParams(splitP);
                            Parser(methodArr[countM].contents);
                        }
                        countM++;
                    }

                    //position command setup
                    String positionS = "Position pen";
                    int resultsPos = String.Compare(command, positionS);

                    //line command setup
                    String lineS = "pen draw";
                    int resultsL = String.Compare(command, lineS);

                    //rect command setup
                    String printS = "print";
                    int resultsP = String.Compare(command, printS);

                    //int command setup
                    String intS = "int";
                    int resultsI = String.Compare(command, intS);

                    //loop setup
                    String loopS = "loop";
                    int resultsLo = String.Compare(command, loopS);

                    //loop setup
                    String ifS = "if";
                    int resultsIf = String.Compare(command, ifS);

                    //method setup
                    String methS = "method";
                    int resultsMeth = String.Compare(command, methS);

                    if (resultsPos == 0)
                    {
                        positionPen(splitP, Brushes.Brown, panel1);
                    }

                    if (resultsL == 0)
                    {
                        drawLine(splitP, Brushes.Brown, panel1);
                    }

                    if (resultsP == 0)
                    {
                        print(parameters);
                    }

                    else if (resultsI == 0)
                    {
                        newVar(splitP);
                    }

                    else if (resultsLo == 0)
                    {
                        Loop(splitP, loopContents);
                    }

                    else if (resultsIf == 0)
                    {
                        If(splitP[0], loopContents);
                    }

                    else if (resultsMeth == 0)
                    {
                        method(splitP, methodContents);
                    }

                    else
                    {
                        int[] splitInts = Array.ConvertAll(splitP, int.Parse);
                        Shape s;
                        s = sf.getShape(command);
                        if (s!=null) {
                            s.set(currLoc.X, currLoc.Y, splitInts[0], splitInts[1]);
                            shapes.Add(s);

                            commLine.Text += command + " drawn at " + currLoc.X + ":" + currLoc.Y + " with width/height of " + splitInts[0] + ":" + splitInts[1];
                        }
                        else
                        {
                            commLine.Text += "ERROR: '" + command + "' is not a valid shape.";
                        }
                        }
                }
            }
        }

        /// <summary>  Method command setup</summary>
        /// <param name="splitP">The split p.</param>
        /// <param name="methodContents">The method contents.</param>
        private void method(string[] splitP, string methodContents)
        {
            string name = splitP[0];
            string[] pAfterName = splitP.Skip(1).ToArray();
            Method newMethod = new Method(name, pAfterName, methodContents);
            methodArr.Add(newMethod);

           // Console.WriteLine(methodArr[0].contents + methodArr[0].name + methodArr[0].parameters[0] + methodArr[0].parameters[1]);
            //gets through to here guaranteed.
        }

        /// <summary>  code for a loop</summary>
        /// <param name="splitP">The split p.</param>
        /// <param name="loopContents_">The loop contents.</param>
        public void Loop(String[] splitP, string loopContents_)
        {
            int loopCount = Int32.Parse(splitP[0]);

            for (int i = 0; i < loopCount; i++)
            {
                Parser(loopContents_);
            }
        }

        /// <summary>  code for an if statement</summary>
        /// <param name="params_">The parameters.</param>
        /// <param name="loopContents_">The loop contents.</param>
        private void If(string params_, string loopContents_)
        {
            char op = '0';
            //check which operator is used within parameters
            foreach (char z in params_)
            {
                if (params_.Contains('='))
                {
                    op = '=';
                    string[] tempSplit = params_.Split(op);
                }

                else if (params_.Contains('>'))
                {
                    op = '>';
                    string[] tempSplit = params_.Split(op);
                }

                else if (params_.Contains('<'))
                {
                    op = '<';

                }
            }

            switch (op)
            {
                case '=':
                    string[] tempSplit1 = params_.Split(op);
                    //clean the string array
                    for (int i = 0; i < tempSplit1.Count(); i++)
                    {
                        tempSplit1[i] = tempSplit1[i].Replace("\r\n", "");
                    }
                    //convert from string arr to int arr
                    int[] tempInts1 = Array.ConvertAll(tempSplit1, int.Parse);
                    //check is smaller

                    //commLine.Text += tempInts.Count();
                    if (tempInts1[0] == tempInts1[1])
                    {
                        Parser(loopContents_);
                    }
                    break;

                case '>':
                    string[] tempSplit2 = params_.Split(op);
                    //clean the string array
                    for (int i = 0; i < tempSplit2.Count(); i++)
                    {
                        tempSplit2[i] = tempSplit2[i].Replace("\r\n", "");
                    }
                    //convert from string arr to int arr
                    int[] tempInts2 = Array.ConvertAll(tempSplit2, int.Parse);
                    //check is smaller

                    //commLine.Text += tempInts.Count();
                    if (tempInts2[0] > tempInts2[1])
                    {
                        Parser(loopContents_);
                    }
                    break;

                case '<':
                    string[] tempSplit3 = params_.Split(op);
                    //clean the string array
                    for (int i = 0; i < tempSplit3.Count(); i++)
                    {
                        tempSplit3[i] = tempSplit3[i].Replace("\r\n", "");
                    }
                    //convert from string arr to int arr
                    int[] tempInts3 = Array.ConvertAll(tempSplit3, int.Parse);
                    //check is smaller

                    //commLine.Text += tempInts.Count();
                    if (tempInts3[0] < tempInts3[1])
                    {
                        Parser(loopContents_);
                    }
                    break;
            }
        }

        /// <summary>Positions the pen.</summary>
        /// <param name="splitP">The split p.</param>
        /// <param name="b">The b.</param>
        /// <param name="p">The p.</param>
        public void positionPen(string[] splitP, Brush b, Panel p)
        {

            int[] splitInts = Array.ConvertAll(splitP, int.Parse);

            currLoc.X = splitInts[0];
            currLoc.Y = splitInts[1];
            commLine.Text += "Pen moved to " + currLoc.X + ":" + currLoc.Y;
        }

        /// <summary>Draws the line.</summary>
        /// <param name="splitP">The split p.</param>
        /// <param name="b">The b.</param>
        /// <param name="p">The p.</param>
        public void drawLine(string[] splitP, Brush b, Panel p)
        {

            int[] splitInts = Array.ConvertAll(splitP, int.Parse);

            g = p.CreateGraphics();
            Pen newPen = new Pen(b);
            g.DrawLine(newPen, currLoc.X, currLoc.Y, splitInts[0], splitInts[1]);
            commLine.Text += "line drawn from " + currLoc.X + ":" + currLoc.Y + " to " + splitInts[0] + ":" + splitInts[1];
        }

        /// <summary>Draws the rect.</summary>
        /// <param name="splitP">The split p.</param>
        /// <param name="b">The b.</param>
        /// <param name="p">The p.</param>
        public void drawRect(string[] splitP, Brush b, Panel p)
        {

            int[] splitInts = Array.ConvertAll(splitP, int.Parse);

            Shape s;
            s = sf.getShape("rect");
            s.set(currLoc.X,currLoc.Y,splitInts[0],splitInts[1]);
            shapes.Add(s);
        }

        /// <summary>Draws the ellipse.</summary>
        /// <param name="splitP">The split p.</param>
        /// <param name="b">The b.</param>
        /// <param name="p">The p.</param>
        public void drawEllipse(string[] splitP, Brush b, Panel p)
        {

            int[] splitInts = Array.ConvertAll(splitP, int.Parse);

            g = p.CreateGraphics();
            Pen newPen = new Pen(b);
            g.DrawEllipse(newPen, currLoc.X, currLoc.Y, splitInts[0], splitInts[0]);
            commLine.Text += "Circle drawn at " + currLoc.X + ":" + currLoc.Y + " with radius of " + splitInts[0];
        }

        /// <summary>Prints the specified parameters.</summary>
        /// <param name="params_">The parameters.</param>
        private void print(String params_)
        {
            commLine.Text += params_;
        }

        /// <summary>  creates new variable</summary>
        /// <param name="splitP">The split p.</param>
        public void newVar(String[] splitP)
        {

            int intValue = Int32.Parse(splitP[1]);

            VarInt newVariable = new VarInt(splitP[0], intValue);
            intArr.Add(newVariable);
        }

        /// <summary>Prints the int array.</summary>
        public void printIntArray()
        {
            foreach (VarInt value in intArr)
            {
                commLine.Text += value.getName() + "::" + value.getValue();
            }
        }

        /// <summary>Handles the Click event of the saveToolStripMenuItem control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String path = sfd.FileName;
                BinaryWriter bw = new BinaryWriter(File.Create(path));
                bw.Write(editor.Text);
                bw.Dispose();
            }

        }

        /// <summary>Handles the Click event of the openToolStripMenuItem control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader bw = new StreamReader(File.OpenRead(ofd.FileName));
                editor.Text = bw.ReadToEnd();
            }
        }

        /// <summary>Handles the Paint event of the panel1 control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        public void panel1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            myPen = new Pen(Color.Black);
            for(int i = 0; i < shapes.Count; i++)
            {
                Shape s;
                s = (Shape)shapes[i];
                if (s != null)
                {
                    s.draw(g);

                }
            }
        }

        /// <summary>Handles the Tick event of the timer1 control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            panel1.Refresh();
        }

    }



}

