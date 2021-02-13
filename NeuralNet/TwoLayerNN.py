import math
import random

class TwoLayerNN:
    OUTPUT_DIMS = 1

    def __init__(self, input_dims, hidden_dims):
        self._weights1 = [[random.uniform(-1, 1) for i in range(input_dims + 1)] for j in range(hidden_dims)]        
        self._weights2 = [[random.uniform(-1, 1) for i in range(hidden_dims + 1)] for j in range(self.OUTPUT_DIMS)]       

    def train(self, train_data, train_labels, learning_rate, epochs):
        for _ in range(epochs):            
            hidden_outputs, outputs = self._forward_pass(train_data)
            for i in range(len(outputs)):                
                output_layer_errors = [o * (1 - o) * (train_labels[i] - o) for o in outputs[i]]   
                hidden_layer_errors = self._calculate_hidden_layer_errors(
                    hidden_outputs[i], self._weights2, output_layer_errors)

                self._update_weights(self._weights2, hidden_outputs[i], output_layer_errors, learning_rate)
                self._update_weights(self._weights1, train_data[i], hidden_layer_errors, learning_rate)
     
    def _calculate_hidden_layer_errors(self, hidden_output, output_weights, output_layer_errors):
        hidden_neurons_errors = []
        for h_idx, h in enumerate(hidden_output):
            weighted_sum = 0
            for j in range(self.OUTPUT_DIMS):
                weighted_sum += output_weights[j][h_idx] * output_layer_errors[j]
            hidden_neurons_errors.append(h * (1 - h) * weighted_sum)
        return hidden_neurons_errors

    def _update_weights(self, weights, prev_layer_value, loss, learning_rate):
        for i in range(len(weights)):
            for j in range(len(weights[0]) - 1):
                weights[i][j] += learning_rate * loss[i] * prev_layer_value[j]
            weights[i][-1] += learning_rate * loss[i]

    def predict(self, test_data):
        _, outputs = self._forward_pass(test_data)
        return outputs

    def _forward_pass(self, data):
        hidden_outputs = []
        outputs = []
        for entry in data:
            out_hidden = self._multiply_weights(self._weights1, entry)
            out_hidden = self._sigmoid(out_hidden)

            out = self._multiply_weights(self._weights2, out_hidden)            
            out = self._sigmoid(out)
            
            hidden_outputs.append(out_hidden)
            outputs.append(out)
            
        return hidden_outputs, outputs
    
    def _multiply_weights(self, weights, entry):
        results = []
        for weight_set in weights:
            result = 0
            for i in range(len(weight_set) - 1):
                result += entry[i] * weight_set[i]
            
            result += weight_set[-1]
            results.append(result)    

        return results

    def _sigmoid (self, values):
        values = [1/(1 + math.e ** (-x)) for x in values]
        return values
        