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

        public Form1()
        {
            MessageBox.Show("This is a 4 input \"AND Gate\" in Neural Network Back-Propagation.", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create a neural network with 4 inputs, 2 hidden neurons, and 1 output neuron
            // Rule of Thumb in determining the number of hidden neurons: ((2/3)*input_size)+output_size.
            // https://www.heatonresearch.com/2017/06/01/hidden-layers.html
            nn = new NeuralNet(4, 3, 1); // 50 Hidden Neuron is quite good
            MessageBox.Show("Neural Network has been initialized!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Training may take a moment press YES to continue.", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
            {
                if (nn == null)
                {
                    MessageBox.Show("Neural Network is not yet initialized!\nClick \"create BPNN\" button first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
                MessageBox.Show($"Finished Training: \nTotal Epoch: {epochs}\nFinal MSE: {totalMSE}", "Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

            try
            {
                nn.setInputs(0, Convert.ToDouble(textBox1.Text));
                nn.setInputs(1, Convert.ToDouble(textBox2.Text));
                nn.setInputs(2, Convert.ToDouble(textBox3.Text));
                nn.setInputs(3, Convert.ToDouble(textBox4.Text));
            } catch
            {
                MessageBox.Show("Invalid Inputs!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            nn.run();
            textBox5.Text = "" + Math.Round(nn.getOuputData(0));
        }
    }
}
