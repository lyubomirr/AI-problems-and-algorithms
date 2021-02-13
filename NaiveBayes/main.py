import csv
from NaiveBayesClassifier import NaiveBayesClassifier

DATASET_FILE = 'house-votes-84.data'

def read_dataset():
	with open(DATASET_FILE, 'r') as csv_file:
		csv_reader = csv.DictReader(csv_file, delimiter=',')
		return list(csv_reader)

def k_fold_cross_validation(k=10):
	dataset = read_dataset()
	classifier = NaiveBayesClassifier()
	one_tenth_of_dataset = int(len(dataset) / k)
	accuracies = []

	for i in range(k):
		test_start_pos = i * one_tenth_of_dataset
		test_end_post = (i + 1) * one_tenth_of_dataset if i < k-1 else len(dataset)
		
		test_dataset = dataset[test_start_pos : test_end_post]
		train_dataset = dataset[0 : test_start_pos] + dataset[test_end_post : len(dataset)]
	
		classifier.train(train_dataset)
		predictions = classifier.predict(test_dataset)

		accuracy = calcuate_accuracy(test_dataset, predictions)
		accuracies.append(accuracy)
		print("Cross validation #" + str(i + 1) + ", accuracy: " + str(accuracy))
	
	print("Average accuracy: " + str(sum(accuracies) / len(accuracies)))

def calcuate_accuracy(test_dataset, predictions):
	correct_predictions = 0
	idx = 0
	for entry in test_dataset:
		if(entry['class-name'] == predictions[idx]):
			correct_predictions += 1
		idx += 1

	return correct_predictions / len(test_dataset)

def main():
	k_fold_cross_validation()

if __name__ == '__main__':
    main()