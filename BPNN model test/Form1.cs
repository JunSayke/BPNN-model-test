using Backprop;
using System.Diagnostics;

namespace BPNN_model_test
{
    public partial class Form1 : Form
    {
        NeuralNet nn;
        // 16 possible input-output combinations for the 4-input AND gate
        double[,] inputs = {
            { 0, 0, 0, 0 }, { 0, 0, 0, 1 }, { 0, 0, 1, 0 }, { 0, 0, 1, 1 },
            { 0, 1, 0, 0 }, { 0, 1, 0, 1 }, { 0, 1, 1, 0 }, { 0, 1, 1, 1 },
            { 1, 0, 0, 0 }, { 1, 0, 0, 1 }, { 1, 0, 1, 0 }, { 1, 0, 1, 1 },
            { 1, 1, 0, 0 }, { 1, 1, 0, 1 }, { 1, 1, 1, 0 }, { 1, 1, 1, 1 }
        };
        double[] desiredOutputs = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0 };
        bool doneTraining = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create a neural network with 4 inputs, 2 hidden neurons, and 1 output neuron
            nn = new NeuralNet(4, 50, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int epochs = 0;
            double thresholdMSE = 0.01;
            double totalMSE;
            //Training loop
            do
            {
                totalMSE = 0;
                for (int i = 0; i < 16; i++)
                {
                    // Set inputs for the current iteration
                    nn.setInputs(0, inputs[i, 0]);
                    nn.setInputs(1, inputs[i, 1]);
                    nn.setInputs(2, inputs[i, 2]);
                    nn.setInputs(3, inputs[i, 3]);

                    // Set the desired output for the current iteration
                    nn.setDesiredOutput(0, desiredOutputs[i]);

                    // Perform learning
                    nn.learn();
                    totalMSE += nn.getMSE();
                }
                Debug.WriteLine("MSE: " + totalMSE);
                Debug.WriteLine("Current Epoch: " + epochs);
                epochs++;
            } while (totalMSE >= thresholdMSE);
            
            Debug.WriteLine("Total epoch: " + epochs);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                // Set inputs for the current iteration
                nn.setInputs(0, inputs[i, 0]);
                nn.setInputs(1, inputs[i, 1]);
                nn.setInputs(2, inputs[i, 2]);
                nn.setInputs(3, inputs[i, 3]);
                
                nn.run();
                Debug.WriteLine(nn.getOuputData(0));
            }

            nn.setInputs(0, Convert.ToDouble(textBox1.Text));
            nn.setInputs(1, Convert.ToDouble(textBox2.Text));
            nn.setInputs(2, Convert.ToDouble(textBox3.Text));
            nn.setInputs(3, Convert.ToDouble(textBox4.Text));
            nn.run();
            textBox5.Text = "" + nn.getOuputData(0);
        }
    }
}
