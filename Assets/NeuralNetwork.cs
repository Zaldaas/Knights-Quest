using UnityEngine;
using System;
using System.IO;

public class NeuralNetwork
{
    private int inputSize;
    private int hiddenSize;
    private int outputSize;

    private float[,] weightsInputHidden;
    private float[,] weightsHiddenOutput;

    private float learningRate = 0.01f;

    public NeuralNetwork(int inputSize = 6, int hiddenSize = 12, int outputSize = 4)
    {
        this.inputSize = inputSize;
        this.hiddenSize = hiddenSize;
        this.outputSize = outputSize;

        // Initialize weights with small random values
        weightsInputHidden = new float[inputSize, hiddenSize];
        weightsHiddenOutput = new float[hiddenSize, outputSize];

        InitializeWeights(weightsInputHidden);
        InitializeWeights(weightsHiddenOutput);
    }

    private void InitializeWeights(float[,] weights)
    {
        for (int i = 0; i < weights.GetLength(0); i++)
        {
            for (int j = 0; j < weights.GetLength(1); j++)
            {
                weights[i, j] = UnityEngine.Random.Range(-1f, 1f);
            }
        }
    }

    public float[] Predict(float[] inputs)
    {
        // Forward pass
        float[] hidden = new float[hiddenSize];
        for (int i = 0; i < hiddenSize; i++)
        {
            float sum = 0f;
            for (int j = 0; j < inputSize; j++)
            {
                sum += inputs[j] * weightsInputHidden[j, i];
            }
            hidden[i] = Sigmoid(sum);
        }

        float[] output = new float[outputSize];
        for (int i = 0; i < outputSize; i++)
        {
            float sum = 0f;
            for (int j = 0; j < hiddenSize; j++)
            {
                sum += hidden[j] * weightsHiddenOutput[j, i];
            }
            output[i] = sum; // For Q-values, no activation
        }

        return output;
    }

    public void UpdateWeights(float reward, float[] nextState)
    {
        // Simple Q-Learning update (not full backpropagation)
        // This is a placeholder for a more sophisticated training method

        // In a full implementation, you would store experiences and perform batch updates
        // Here, we perform a rudimentary update based on the reward

        // Example: Increase weights for actions that led to positive rewards
        // and decrease for negative rewards

        // This is highly simplified and not recommended for complex tasks

        // Placeholder: Adjust all weights slightly based on the reward
        for (int i = 0; i < weightsInputHidden.GetLength(0); i++)
        {
            for (int j = 0; j < weightsInputHidden.GetLength(1); j++)
            {
                weightsInputHidden[i, j] += learningRate * reward * UnityEngine.Random.Range(-0.1f, 0.1f);
            }
        }

        for (int i = 0; i < weightsHiddenOutput.GetLength(0); i++)
        {
            for (int j = 0; j < weightsHiddenOutput.GetLength(1); j++)
            {
                weightsHiddenOutput[i, j] += learningRate * reward * UnityEngine.Random.Range(-0.1f, 0.1f);
            }
        }
    }

    private float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

    public void SaveWeights(string path)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            // Save weightsInputHidden
            writer.WriteLine("weightsInputHidden");
            for (int i = 0; i < weightsInputHidden.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < weightsInputHidden.GetLength(1); j++)
                {
                    line += weightsInputHidden[i, j].ToString("F4") + ",";
                }
                writer.WriteLine(line.TrimEnd(','));
            }

            // Save weightsHiddenOutput
            writer.WriteLine("weightsHiddenOutput");
            for (int i = 0; i < weightsHiddenOutput.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < weightsHiddenOutput.GetLength(1); j++)
                {
                    line += weightsHiddenOutput[i, j].ToString("F4") + ",";
                }
                writer.WriteLine(line.TrimEnd(','));
            }
        }
    }

    public void LoadWeights(string path)
    {
        if (!File.Exists(path))
            return;

        string[] lines = File.ReadAllLines(path);
        int lineIndex = 0;

        // Load weightsInputHidden
        if (lines[lineIndex] == "weightsInputHidden")
        {
            lineIndex++;
            for (int i = 0; i < inputSize; i++)
            {
                string[] parts = lines[lineIndex].Split(',');
                for (int j = 0; j < hiddenSize; j++)
                {
                    weightsInputHidden[i, j] = float.Parse(parts[j]);
                }
                lineIndex++;
            }
        }

        // Load weightsHiddenOutput
        if (lines[lineIndex] == "weightsHiddenOutput")
        {
            lineIndex++;
            for (int i = 0; i < hiddenSize; i++)
            {
                string[] parts = lines[lineIndex].Split(',');
                for (int j = 0; j < outputSize; j++)
                {
                    weightsHiddenOutput[i, j] = float.Parse(parts[j]);
                }
                lineIndex++;
            }
        }
    }
}
