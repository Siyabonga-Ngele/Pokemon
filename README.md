# Pokemon App

## About

This app includes features to navigate through pages, filter Pokémon by name and weight, and display detailed information about selected Pokémon.

## How it works

### Data Retrieval:
I utilized the PokeAPI to fetch Pokémon data by making API requests using the HttpClient in your PokeClient class. The GetPokemon method sends requests to the PokeAPI and retrieves Pokémon information based on the given Pokémon ID.

![Screenshot 2023-11-06 021949](https://github.com/Siyabonga-Ngele/Pokemon/assets/93044587/591bb316-4eb9-45fb-88d8-5e3624c43f04)

### User Interface:
I create a user interface for interacting with the program. This interface is a command-line interface (CLI) implemented in C#.

### Displaying Pokémon Information:
- I display Pokémon information, such as the name, weight, height, and other attributes, in a tabular format in the console. The DisplayList method is responsible for showing a list of Pokémon with their details.
- I also display detailed stats of each pokemon when an indivdual pokemon is selected using the printCard method in the application.

![Screenshot 2023-11-06 022113](https://github.com/Siyabonga-Ngele/Pokemon/assets/93044587/4bfd34be-99f4-4460-8e78-87acea45ab09)

### Filtering:
- I implement a filtering system that allows users to filter Pokémon based on different criteria. Users can filter Pokémon by name and weight, as indicated by the "Filter by Name" and "Filter by Weight" options. This filtering allows users to narrow down the list of displayed Pokémon based on their preferences.
- Side note: these functions require optimization as they seek to view the details of all 1000+ pokemon which results in long filtering times

### User Interaction:

- Users can view a list of Pokémon, and this list is paginated to display 20 Pokémon at a time, with options to navigate to the next or previous pages using the characters 'n' and 'p' .
- Users can select and view detailed information about a specific Pokémon by first selecting the letter 'x' and then entering its Pokémon number upon prompt.
- Users can filter Pokémon by name or weight to find Pokémon that match specific criteria using the characters 'f' and 'g'.

### Caching

I have implemented an alternative to caching. for example when a user selects a pokemon already present in the list we do not issue any api request rather we find it's information via the list.

