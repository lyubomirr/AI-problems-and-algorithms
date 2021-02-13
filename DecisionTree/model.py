import math
import operator
from data import *

class TreeNode:
    def __init__(self, feature_name):
        self.feature_name = feature_name
        self.children = {} 

class ID3DecisionTreeClassifier:
    def __init__(self):
        self._root = None

    def train(self, train_dataset, minimum_examples = 1):
        root_feature = self._get_feature_with_highest_information_gain(train_dataset, FEATURES)
        self._root = TreeNode(root_feature)
        self._id3(self._root, train_dataset, FEATURES, minimum_examples)

    def _get_class_probabilities(self, dataset):
        dataset_length = len(dataset)
        class_probabilities = {}

        for class_name in CLASSES:
            class_count = sum(1 for x in dataset if x['class'] == class_name)
            class_probabilities[class_name] = class_count / dataset_length if dataset_length > 0 else 0

        return class_probabilities

    def _get_feature_with_highest_information_gain(self, dataset, features):
        dataset_entropy = self._get_entropy(dataset)
        dataset_length = len(dataset)
        feature_frequencies = self._get_feature_frequencies(dataset)
        information_gains = {}

        for feature in features:
            information_gains[feature] = dataset_entropy
            for feature_value in FEATURE_VALUES[feature]:
                subset = [entry for entry in dataset if entry[feature] == feature_value]
                feature_value_probability = feature_frequencies[(feature, feature_value)] \
                                            / dataset_length if(feature, feature_value) in feature_frequencies else 0

                information_gains[feature] -= feature_value_probability * self._get_entropy(subset)

        # Get the feature with maximum information gain.
        return max(information_gains.items(), key=operator.itemgetter(1))[0]

    def _get_entropy(self, dataset):
        class_probabilities = self._get_class_probabilities(dataset)
        entropy = 0
        for class_name in CLASSES:
            if(class_probabilities[class_name] > 0):
                entropy += -class_probabilities[class_name] * math.log(class_probabilities[class_name], 2)
        return entropy

    def _get_feature_frequencies(self, dataset):
        feature_frequencies = {}

        for entry in dataset:
            for feature in entry:
                if feature == 'class':
                    continue
                if (feature, entry[feature]) in feature_frequencies:
                    feature_frequencies[(feature, entry[feature])] += 1
                else:
                    feature_frequencies[(feature, entry[feature])] = 1

        return feature_frequencies

    def _id3(self, node, dataset, features, minimum_examples):
        for feature_value in FEATURE_VALUES[node.feature_name]:            
            subset = [entry for entry in dataset if entry[node.feature_name] == feature_value]
            if(len(subset) < minimum_examples):
                self._set_most_common_class_for_child(node, feature_value, dataset)
            elif(self._is_dataset_of_terminal_node(subset)):
                # They all have the same class just get it from the first example
                node.children[feature_value] = subset[0]["class"]
            else:
                new_feautres = [feature for feature in features if feature != node.feature_name]
                if(len(new_feautres) == 0):                    
                    self._set_most_common_class_for_child(node, feature_value, dataset)
                else:
                    next_feature = self._get_feature_with_highest_information_gain(subset, new_feautres)
                    new_node = TreeNode(next_feature)
                    node.children[feature_value] = new_node
                    self._id3(new_node, subset, new_feautres, minimum_examples)

    def _set_most_common_class_for_child(self, node, feature_value, dataset):
        most_common_class =  max(self._get_class_probabilities(dataset).items(),
                                        key=operator.itemgetter(1))[0]
        node.children[feature_value] = most_common_class

    def _is_dataset_of_terminal_node(self, dataset):
        return self._get_entropy(dataset) == 0       

    def predict(self, test_dataset):
        predicted_classes = []
        for entry in test_dataset:            
            predicted_classes.append(self._traverse_tree(self._root, entry))
        return predicted_classes

    def _traverse_tree(self, node, entry):
        entry_feature_value = entry[node.feature_name]
        if(isinstance(node.children[entry_feature_value], TreeNode)):
            return self._traverse_tree(node.children[entry_feature_value], entry)
        else:
            return node.children[entry_feature_value]