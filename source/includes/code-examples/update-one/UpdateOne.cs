// Updates the first document that matches a query filter by using the C# driver

using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace CSharpExamples.UsageExamples.UpdateOne;

public class UpdateOne
{
    private static IMongoCollection<Restaurant> _restaurantsCollection;
    private const string MongoConnectionString = "<connection string>";

    public static void Main(string[] args)
    {
        Setup();

        // Prints extra space for console readability 
        Console.WriteLine();

        // Updates one document by using a helper method
        var syncResult = UpdateOneRestaurant();
        Console.WriteLine($"Updated documents: {syncResult.ModifiedCount}");
        ResetSampleData();
    }

    private static UpdateResult UpdateOneRestaurant()
    {
        // start-update-one
        const string oldValue = "Bagels N Buns";
        const string newValue = "2 Bagels 2 Buns";

        // Creates a filter for all documents with a "name" of "Bagels N Buns"
        var filter = Builders<Restaurant>.Filter
            .Eq(restaurant => restaurant.Name, oldValue);

        // Creates instructions to update the "name" field of the first document
        // that matches the filter
        var update = Builders<Restaurant>.Update
            .Set(restaurant => restaurant.Name, newValue);

        // Updates the first document that has a "name" value of "Bagels N Buns"
        return _restaurantsCollection.UpdateOne(filter, update);
        // end-update-one
    }

    private static void Setup()
    {
        // Allows automapping of the camelCase database fields to models 
        var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

        // Establishes the connection to MongoDB and accesses the "sample_restaurants" collection
        var mongoClient = new MongoClient(MongoConnectionString);
        var restaurantsDatabase = mongoClient.GetDatabase("sample_restaurants");
        _restaurantsCollection = restaurantsDatabase.GetCollection<Restaurant>("restaurants");
    }

    private static void ResetSampleData()
    {
        var filter = Builders<Restaurant>.Filter
            .Eq(restaurant => restaurant.Name, "2 Bagels 2 Buns");

        var update = Builders<Restaurant>.Update
            .Set(restaurant => restaurant.Name, "Bagels N Buns");

        _restaurantsCollection.UpdateOne(filter, update);
    }
}

public class Restaurant
{
    public ObjectId Id { get; set; }

    public string Name { get; set; }

    [BsonElement("restaurant_id")]
    public string RestaurantId { get; set; }

    public string Cuisine { get; set; }

    public Address Address { get; set; }

    public string Borough { get; set; }

    public List<GradeEntry> Grades { get; set; }
}

public class Address
{
    public string Building { get; set; }

    [BsonElement("coord")]
    public double[] Coordinates { get; set; }

    public string Street { get; set; }

    [BsonElement("zipcode")]
    public string ZipCode { get; set; }
}

public class GradeEntry
{
    public DateTime Date { get; set; }

    public string Grade { get; set; }

    public float Score { get; set; }
}
