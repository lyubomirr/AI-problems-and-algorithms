import math
import random

class SingleLayerNN:
    def __init__(self, input_dims):
        self._weights = [random.uniform(-1, 1) for i in range(input_dims + 1)]

    def train(self, train_data, train_labels, learning_rate, epochs):
        for _ in range(epochs):
            output = self._forward_pass(train_data)
            for idx, prediction in enumerate(output):
                error = prediction * (1 - prediction) * (train_labels[idx] - prediction)
                self._update_weights(train_data[idx], error, learning_rate)
    
    def _update_weights(self, prev_layer_value, error, learning_rate):
        for i in range(len(self._weights) - 1):
            self._weights[i] += learning_rate * error * prev_layer_value[i]

        self._weights[-1] += learning_rate * error


    def predict(self, test_data):
        return self._forward_pass(test_data)

    def _forward_pass(self, data):
        output = []
        for entry in data:
            out = self._multiply_weights(entry)
            out = self._sigmoid(out)
            output.append(out)
        
        return output
    
    def _multiply_weights(self, entry):
        result = 0
        for i in range(len(self._weights) - 1):
            result += entry[i] * self._weights[i]

        result += self._weights[-1]
        return result

    def _sigmoid (self, x):
        return 1/(1 + math.e ** (-x))
        