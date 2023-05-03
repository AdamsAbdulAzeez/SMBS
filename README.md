![image](https://user-images.githubusercontent.com/39003759/183617916-7ae2eb60-dc1a-493a-aca1-a21cde3f9514.png)

# Architecture

The aplication is an adapted WPF desktop, MVVM (PRISM) application that relies on dependency injection. The current roadmap and state of the board is available on [Poject Board](https://github.com/orgs/cyphercrescent/projects/5)

## Commands

For better organisation and single responsibility. Commands are created as separate classes and referenced in the view model. This encourages testability, and makes it obvious when certain commands are doing more than they should.


## Events

Events play a critical part in our attempt to decouple the system. Classes have CLR events that allow subscribers act when an internal state changes. This could be domain related event like `GeometryChanged` or `HistoryMatchingResultAccepted`. Outside of this, `modules` and components communicate using the `IEventAggregator` from PRISM. 


## Features

What defines a feature ?

A difficult question to answer. But as a rule of thumb, the `Features` folder of the app defines top level `modules` or features in the application. Within each folder in the `Features` every related or sub-feature is a folder containing it's views, view models, converters, events etc. This follows the `Principle of Proximity`

<blockquote>
The principle of proximity focuses on how well organized your code is with respect to readability and change. Proximity implies that functions that are changed together are moved closer together. Proximity is both a design principle and a heuristic for refactoring hotspots toward code that's easier to understand.

[O'Reilly Read more](https://www.oreilly.com/library/view/software-design-x-rays/9781680505795/f_0031.xhtml#:~:text=The%20principle%20of%20proximity%20focuses,code%20that%27s%20easier%20to%20understand.)

</blockquote>


## Storage, Persistent Objects vs Core Objects

The application is using a flat file storage. This can be extended to other database/storage types by providing an implemention for the `IModelStore` interface.

As storage is a major concern and dependency, and considering several possibilities to migrate, update or change storage media in the nearest future, the `IModelStore` defines the contract we expect from any storage media. 

Persistent models define schema representations for storage. While Business/core models are rich models that contain events, methods and other usefull attributes of the model. While it is sometimes okay in smaller applications to persist business objects directly, it creates tight dependency to the database implementations and future modifications, optimisations and changes ripple from storage all the way to the UI.

Example

````csharp
\\ Rectangle Schema for a Rectangle Table in an SQL database
class Rectangle {
    public double Width { get;set; }
    public double Height { get;set;}
    public virtual IList<DataPoint> Points { get; set; }
    // The Rectangle table schema definition is only concerned with storage specifics of the Rectangle object. 
    // It can make certain tweaks/optimisations depending on the database. E.g making the properties virtual as in the case of NHibernate.
    // It is however not interested in events because stored records do not react or raise events.
}

\\ Business model used in application
class Rectangle {
    public Length Width { get;set; }
    public Length Height { get;set; }
    public Dictionary<int, DataPoints> Points {get;set;}
    public event Action OnSizeChanged;
    // The Rectangle table schema definition represents the storage datatypes as double
    // (as required) by the column type of the database (in the case of SQL). But the business object defines the types as `Length`.
    // This means the business object is unit aware, whereas the persistent object is unit dumb.
}
```



In a SQL The Rectable table schema definition represents the Points Collection as a virtual IList<T>. This may be due to underlying concerns from an ORM but isn't the most performant representation of the attribute. The business model representes the attribute as a Dictionary which allows faster indexing based on the application use-case.

    
