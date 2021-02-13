import argparse
from clusterize import k_means

def main():
    args = parse_args()
    dataset = read_dataset(args.dataset)
    k_means(args.k, dataset, args.i)

def parse_args():
    parser = argparse.ArgumentParser()
    
    parser.add_argument("-dataset", default="normal", 
        choices=["normal", "unbalance"], help="Choose which dataset to use.")
    parser.add_argument("-k", default=4, type=int, help="Number of clusters.")
    parser.add_argument("-i", default=10, type=int, help="Number of iterations.")

    return parser.parse_args()

def read_dataset(dataset):
    if(dataset == "normal"):
        dataset_file_path = "./data/normal/normal.txt"
    else:
        dataset_file_path = "./data/unbalance/unbalance.txt"

    with open(dataset_file_path, newline='') as dataset_file:
        rows = [line.split() for line in dataset_file]
        dataset = [[float(coordinate) for coordinate in position] for position in rows]
        return dataset
        
if __name__ == '__main__':
    main()