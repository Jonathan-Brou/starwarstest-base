# starwarstest-base

Base Star Wars API test project

# Test Requirements

- The API should get data from the swapi Star Wars API: https://swapi.dev/documentation
    - List all characters from the original three films
    - Sort the list of characters by Film (ordered by film #) -> Planet (alphabetical) -> Character Name (alphabetical)
    - The character name is one field in surname - given-name order.
- The front-end UI should display a button to get the character data and display the character data in the UI.
    - The UI should only call the API in this repo. The UI should **not** call directly to the Swapi API.
    - Both synchronous and asynchronous calls to the API are acceptable. Async is preferable. The method of async does not matter.
    - (Optional) Allow the user to download a CSV file of the data. If you do this, the CSV should match the displayed data **exactly**.

You may use one of the C# libraries that are compatible with the Swapi API.

Bonus points are **not** awarded for going beyond the requirements given.

Try to use industry-standard project structure. Some variation in structure is expected.
