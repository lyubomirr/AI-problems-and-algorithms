import random
import math
import matplotlib.pyplot as plt
from itertools import accumulate

def k_means(k, dataset, iterations):
    best_centroids, best_clusters = k_means_core(k, dataset)
    best_wssd = calculate_total_wssd(best_centroids, best_clusters)

    for i in range(iterations - 1):
        centroids, clusters = k_means_core(k, dataset)
        wssd = calculate_total_wssd(centroids, clusters)

        if(wssd < best_wssd):
            best_centroids, best_clusters, best_wssd = centroids, clusters, wssd

    print_data(best_centroids, best_clusters)

def k_means_core(k, dataset):
    centroids = k_means_plus_plus_centroids(k, dataset)
    current_centroid_assignments = []
    has_changed = True

    while has_changed == True:
        current_centroid_assignments, has_changed = \
            calculate_closest_centroid(centroids, dataset, current_centroid_assignments)
        
        cluster_dict = parse_assignments_to_dict(dataset, current_centroid_assignments, k)
        if(has_changed == True):
            # Don't need to calculate new centroids as nothing changed in clusters.
            centroids = calculate_centroids_new_position(k, dataset, cluster_dict)
        
    return centroids, cluster_dict

def k_means_plus_plus_centroids(k, dataset):
    centroids = []
    centroids.append(random.choice(dataset))

    for i in range(k-1):
        acc_probabilities = get_accumulated_probabilities(centroids, dataset)
        new_centroid_idx = get_choice_from_probabilities(acc_probabilities)
        centroids.append(dataset[new_centroid_idx])
    
    return centroids
                
def get_accumulated_probabilities(centroids, dataset):
    distances = [min([calculate_squared_eucledian_distance(point, centroid) for centroid in centroids]) 
                for point in dataset]
    sum_of_distances = sum(distances)
    probabilities = [dist/sum_of_distances for dist in distances]
    return list(accumulate(probabilities))

def get_choice_from_probabilities(accumulated_probabilities):
    choice = random.uniform(0, 1)
    for idx, prob in enumerate(accumulated_probabilities):
        if choice <= prob:
            return idx 

def calculate_closest_centroid(centroids, dataset, prev_centroid_assignments):
    has_changed = False
    centroid_assignments = []

    for i, point in enumerate(dataset):
        min_distance = None
        min_centroid_idx = -1

        for j, centroid in enumerate(centroids):
            distance = calculate_squared_eucledian_distance(point, centroid)
            if(min_distance == None or distance < min_distance):
                min_distance = distance
                min_centroid_idx = j
        
        centroid_assignments.append(min_centroid_idx)
        if (len(prev_centroid_assignments) == 0 # Initial calculation
            or prev_centroid_assignments[i] != min_centroid_idx):
            has_changed = True

    return centroid_assignments, has_changed


def calculate_squared_eucledian_distance(point_a, point_b):
    result = 0
    dims = len(point_a)

    for i in range(dims):
        result += (point_a[i] - point_b[i]) ** 2

    return result

def parse_assignments_to_dict(dataset, centroid_assignments, centroid_count):
    cluster_dict = {}
    for i in range(centroid_count):
        cluster_dict[i] = []

    for i, centroid_number in enumerate(centroid_assignments):        
        cluster_dict[centroid_number].append(dataset[i])

    return cluster_dict

def calculate_centroids_new_position(centroid_count, dataset, cluster_dict):
    new_centroids = []
    for i in range(centroid_count):
        points = cluster_dict[i]
        sum_x = 0
        sum_y = 0

        for point in points:
            sum_x += point[0]
            sum_y += point[1]

        avg_x = sum_x / len(points)
        avg_y = sum_y / len(points)

        new_centroids.append([avg_x, avg_y])
    
    return new_centroids

def calculate_total_wssd(centroids, cluster_dict):
    total_wssd = 0
    for centroid_number in cluster_dict:
        for point in cluster_dict[centroid_number]:
            total_wssd += calculate_squared_eucledian_distance(point, centroids[centroid_number])

    return total_wssd

def print_data(centroids, cluster_dict):
    # Plot the centroids
    for centroid_coords in centroids:
        plt.plot(centroid_coords[0], centroid_coords[1], marker="X", markersize=8, c="red")

    for i in range(len(centroids)):
        color = [random.uniform(0, 1), random.uniform(0, 1), random.uniform(0, 1)]
        xs = [point[0] for idx, point in enumerate(cluster_dict[i])]
        ys = [point[1] for idx, point in enumerate(cluster_dict[i])]

        plt.scatter(xs, ys, c=[color])

    plt.show()
