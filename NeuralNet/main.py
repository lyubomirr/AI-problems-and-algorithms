from SingleLayerNN import SingleLayerNN
from TwoLayerNN import TwoLayerNN

def evaluate_one_layer_nn(train_dataset, train_labels, test_dataset, test_labels):
    nn = SingleLayerNN(input_dims=2)
    nn.train(train_dataset, train_labels, 5, 5000)

    predictions = [1 if out >= 0.5 else 0 for out in nn.predict(test_dataset)]
    return calcuate_accuracy(test_labels, predictions)

def evaluate_or_operation():
    train_dataset = [[0,0], [1,0], [0,1], [1,1]]
    train_labels = [0, 1, 1, 1]

    test_dataset = [[1,1], [0,0], [0,1], [1,0]]
    test_labels = [1, 0, 1 ,1]

    accuracy = evaluate_one_layer_nn(train_dataset, train_labels, test_dataset, test_labels)
    print("Accuracy for OR operation: " + str(accuracy))

def evaluate_and_operation():
    train_dataset = [[0,0], [1,0], [0,1], [1,1]]
    train_labels = [0, 0, 0, 1]

    test_dataset = [[1,1], [0,0], [0,1], [1,0]]
    test_labels = [1, 0, 0, 0]

    accuracy = evaluate_one_layer_nn(train_dataset, train_labels, test_dataset, test_labels)
    print("Accuracy for AND operation: " + str(accuracy))
    
def evaluate_xor_operation():
    train_dataset = [[0,0], [1,0], [0,1], [1,1]]
    train_labels = [0, 1, 1, 0]

    test_dataset = [[1,1], [0,0], [0,1], [1,0]]
    test_labels = [0, 0, 1, 1]

    nn = TwoLayerNN(input_dims=2, hidden_dims=3)
    nn.train(train_dataset, train_labels, 5, 5000)
    
    predictions = [[1 if o > 0.5 else 0 for o in output] for output in nn.predict(test_dataset)]
    #Output has always 1 dimension so just get it
    predictions = [p[0] for p in predictions]
    accuracy = calcuate_accuracy(test_labels, predictions)
    print("Accuracy for XOR operation: " + str(accuracy))

def calcuate_accuracy(test_labels, predictions):
	correct_predictions = 0
	idx = 0
	for idx, test_label in enumerate(test_labels):
		if(test_label == predictions[idx]):
			correct_predictions += 1
		idx += 1

	return correct_predictions / len(test_labels)

def main():
    evaluate_and_operation()
    evaluate_or_operation()
    evaluate_xor_operation()

if __name__ == '__main__':
    main()