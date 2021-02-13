import csv
from collections import Counter

DATASET_FILE = 'breast-cancer.data'

def read_dataset():
	with open(DATASET_FILE, 'r') as csv_file:
		csv_reader = csv.DictReader(csv_file, delimiter=',')
		return list(csv_reader)

def fill_in_missing_features(dataset):
	modes = {}

	for entry in dataset:
		for feature in entry:
			if(entry[feature] == "?"):
				if(not feature in modes):
					modes[feature], _ = get_mode(dataset, feature)				
				entry[feature] = modes[feature]
	
	return dataset

def get_mode(dataset, feature_name):
	feature_values = [entry[feature_name] for entry in dataset if entry[feature_name] != "?"]
	return Counter(feature_values).most_common(1)[0]