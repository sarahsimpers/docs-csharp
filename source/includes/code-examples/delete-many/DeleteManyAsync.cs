// Asynchronously deletes multiple documents from a collection by using the C# driver

using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace CSharpExamples.UsageExamples.DeleteMany;

public class DeleteManyAsync
{
    private static IMongoCollection<Restaurant> _restaurantsCollection;
    private const string MongoConnectionString = "<connection string>";

    public static async Task Main(string[] args)
    {
        Setup();

        var filter = Builders<Restaurant>.Filter
            .Eq(r => r.Borough, "Brooklyn");

        var docs = _restaurantsCollection.Find(filter).ToList();

        // Deletes documents by using builders
        Console.WriteLine("Deleting documents...");
        var result = await DeleteMultipleRestaurantsBuilderAsync();

        Console.WriteLine($"Deleted documents: {result.DeletedCount}");

        Restore(docs);

        return result;
    }

    // Deletes all documents that have a Borough value of "Brooklyn"
    private static async Task<DeleteResult> DeleteMultipleRestaurantsBuilderAsync()
    {
        // start-delete-many-async
        // Creates a filter for all documents that have a
        // "borough" value of "Brooklyn"
        var filter = Builders<Restaurant>.Filter
            .Eq(r => r.Borough, "Brooklyn");

        // Asynchronously deletes all documents that match the filter
        return await _restaurantsCollection.DeleteManyAsync(filter);
        // end-delete-many-async
    }

    private static void Restore(IEnumerable<Restaurant> docs)
    {
        _restaurantsCollection.InsertMany(docs);
        Console.WriteLine("Resetting sample data...done.");
    }

    private static void Setup()
    {
        // Allows automapping of the camelCase database fields to models 
        var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

        // Establishes the connection to MongoDB and accesses the "restaurants" collection
        var mongoClient = new MongoClient(MongoConnectionString);
        var restaurantsDatabase = mongoClient.GetDatabase("sample_restaurants");
        _restaurantsCollection = restaurantsDatabase.GetCollection<Restaurant>("restaurants");
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
