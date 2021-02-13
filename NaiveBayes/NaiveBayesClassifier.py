import math
import classes
import data

class NaiveBayesClassifier:
    def __init__(self):
        self.__priorProbabilities = {}
        self.__conditionalProbabilities = {}

    def train(self, train_dataset, smoothingAlpha = 1):
        self.__priorProbabilities = {}
        self.__conditionalProbabilities = {}

        classCounts = {
            classes.REPUBLICAN: sum(1 for x in train_dataset if x['class-name'] == classes.REPUBLICAN),
            classes.DEMOCRAT: sum(1 for x in train_dataset if x['class-name'] == classes.DEMOCRAT)
        }

        self.__init_prior_probabilities(classCounts, len(train_dataset), smoothingAlpha)

        featureFrequencies = self.__get_feature_frequencies(train_dataset)
        for (feature, value, className) in featureFrequencies:
            count = featureFrequencies[(feature, value, className)]
            totalCount = classCounts[className] + len(data.featureValues) * smoothingAlpha
            self.__conditionalProbabilities[(feature, value, className)] = count / totalCount

    def __init_prior_probabilities(self, classCounts, trainDatasetLength, smoothingAlpha):
        #Using Laplace smoothing
        totalEntries = trainDatasetLength + smoothingAlpha * len(data.classNames)
        self.__priorProbabilities[classes.REPUBLICAN] = (classCounts[classes.REPUBLICAN] + smoothingAlpha) / totalEntries
        self.__priorProbabilities[classes.DEMOCRAT] = (classCounts[classes.DEMOCRAT] + smoothingAlpha) / totalEntries

    def __get_feature_frequencies(self, train_dataset):
        featureFrequencies = self.__init_feature_frequencies()

        for entry in train_dataset:
            className = entry['class-name']
            for feature in entry:
                if feature == 'class-name':
                    continue
                featureFrequencies[(feature, entry[feature], className)] += 1

        return featureFrequencies

    def __init_feature_frequencies(self):
        frequencies = {}
        
        #Initialize them all to one as we use Laplace smoothing to avoid zero probability problem.
        for feature in data.features:
            for featureValue in data.featureValues:
                for className in data.classNames:
                    frequencies[(feature, featureValue, className)] = 1

        return frequencies


    def predict(self, test_dataset):
        predictedClasses = []
        for entry in test_dataset:
            democratScore = math.log(self.__priorProbabilities[classes.DEMOCRAT])
            republicanScore = math.log(self.__priorProbabilities[classes.REPUBLICAN])

            for feature in entry:
                if feature == 'class-name':
                    continue
                democratScore += math.log(
                    self.__conditionalProbabilities[(feature, entry[feature], classes.DEMOCRAT)])
                republicanScore += math.log(
                    self.__conditionalProbabilities[(feature, entry[feature], classes.REPUBLICAN)])
        
            if democratScore > republicanScore:
                predictedClasses.append(classes.DEMOCRAT)
            else:
                predictedClasses.append(classes.REPUBLICAN)
        
        return predictedClasses

