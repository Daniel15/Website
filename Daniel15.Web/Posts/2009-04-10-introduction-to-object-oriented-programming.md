---
id: 162
title: Daniel15's Introduction to Object-Oriented Programming
published: true
publishedDate: 2009-04-10 22:57:44Z
lastModifiedDate: 2009-04-10 22:57:44Z
categories:
- C#
- HIT2302
- Programming

---

Well, back to posting coding-related blog posts, for now anyways :P. Seeing as a lot of people seem to be confused by Object Oriented Programming, I thought I'd post a quick (or maybe not so quick) post about what OOP is, the main features, and how it can benefit you. This is paraphrased from an assignment I had on OOP last semester at university. I use C# code examples throughout this, but the concepts are very similar in other languages. Note that in this post, I assume you know the basics of programming, and just want to learn more about object orientation.

Now, let's begin looking at what OOP actually means. At its core, the Object Oriented paradigm consists of **classes** and **objects**. A class is a “thing” or entity that has a purpose, and an **object** is an instance of this entity. For example, a Car would be a class, and **my car** would be an object (instance of the Car class).  

<!--more-->

## Classes in C#
Before we can cover any of the OO principles with code examples, we need to go through some very simple code. A class in C# is made using the class keyword:  

```csharp
public class Car
{
	// ...
}
```  

The "*public*" is an **access modifier**. The available access modifiers for classes in C# are:

* **public** – No restrictions on accessibility of the class. It can be used from anywhere.
* **private** – Can only be accessed by code in the same class.
* **protected** – Can only be accessed by code in the same class, or in a derived class (see **inheritance**, below).
* **internal** – Can be accessed by any code in the same assembly (basically, the same file), but not from another assembly.

Methods in the class are specified like this:  

```csharp
public virtual void StartCar()
{
	// ...
}
```  

Like above, *public* is an access modifier. The **virtual** keyword means that this method can be overridden in child classes  (this is explained under “inheritance” below). “void” is the return type. A method that returns void basically returns nothing (the same as void in languages like C#, or procedures [as opposed to functions] in Pascal and Visual Basic). Some common return types include string, int, uint (unsigned int), long, ulong, float (Single-precision floating point number, 4 bytes) and double (Double-precision floating point number, 8 bytes)

# Core OO principles
With all that said, let's jump into the core OOP concepts. There are three “pillars” of OOP; these are the core principles of Object Oriented Programming. These are Encapsulation, Inheritance and Polymorphism. In addition to these, there are several other core ideas.

## Abstraction
An abstraction is basically an idea, something that we think about to simplify things. In our car example (which will be used throughout this portfolio), a **Car** is an abstraction.

Encapsulation, Inheritance and Polymorphism (the pillars of OOP) are all abstractions.

## Object Composition
Object composition is basically a way to combine simpler objects into larger, more complex ones. This is known as a “**has-a**” relationship. For example, a **Car** object might contain an Engine, Wheels, etc. We don't need to know how all these other smaller objects work, but only how to interface with them (this is directly related to Encapsulation, see below).

## Encapsulation
Encapsulation is the “hiding” of internal details of a class. This makes the class “friendlier”, and hides all its internals from the user. For example, a “car” class in C# could look something like this:  

```csharp
public class Car
{
	public virtual void StartCar()
	{
		// Do whatever the car needs to do to start
	}

	// More class members here
}
```  

We can use this class without knowing exactly how it starts the car, just that it starts it:  

```csharp
Car myCar = new Car();
myCar.StartCar();
```  

This also ensures that code outside the class can't access data that it shouldn't be allowed to touch. Allowing free access to all **fields** (values that the object “knows”) could lead to data corruption, as the object could be in an unknown state. For example, the car could have a field that says how much petrol is left in its petrol tank. We want the car to know how much petrol is left, but we don't want other classes to be able to just change this value. In C#, we use private variables for this purpose:  

```csharp
public class Car
{
	private int _petrol;
	// ...
}
```  

We could then have a **property** to access this, and choose to only have a getter (no setter) for it, meaning that it can only be read:  

```csharp
public class Car
{
	private int _petrol;
	public int Petrol
	{
			get { return _petrol; }
	}
	// ...
}
```

## Inheritance
Inheritance basically allows us to build new classes which are based on existing classes. The new (lower-level) classes inherit all the functionality of those above it, and can also add new functionality. For example, we cam make a **SportsCar** class that inherits from Car:  

```csharp
public class SportsCar : Car
{
	// ...
}
```  

The SportsCar class will automatically have all the members that Car had (the petrol field, StartCar method, etc.), as well as any new members we define. To override a method in the Car class, we can use the **override** keyword. As long as the function is defined with “virtual”, this will work fine:  

```csharp
public override void StartCar()
{
	// ...
}
```  

In fact, we could take this abstraction even further, and have a **Vehicle** class which the Car inherits from. Then we could have other vehicles (e.g. Bike, Bus, etc.) which all have common elements, and can be treated in the same way (polymorphism, described below)

## Polymorphism
Polymorphism is the ability to access multiple different types of objects using the same methods. For instance, above, we defined SportsCar as a **derived class** of Car. We can treat SportsCar instances (objects) in the same way as we treat Cars, because the compiler knows that a SportsCar **is a** car:  

```csharp
Car myCar = new Car();
Car mySportsCar = new SportsCar();
myCar.StartCar();
mySportsCar.StartCar();
```  

The two types of cars might start very differently, but they're both called using the same StartCar method.

# More OO stuffs
## Constructors
A constructor is a function that's called when an instance of the class is created. They're usually used to set parameters at creation time. The constructor is a method with the same name as the class. For example, in our **Car** class, we could have something like this:  

```csharp
public class Car
{
	private int _petrol;

	public Car(int petrol)
	{
		_petrol = petrol;
	}
}
```  

This means that when the Car class is created, we'd need to pass it one argument (not 0, not more than one).  

```csharp
Car myCar = new Car(100);
```  

We can give a class more than one constructor. In this case, each constructor is differentiated by the number of parameters. If the class is created with one parameter, the constructor with one parameter is called. Same with if the class is created with two parameters, the constructor with two parameters is called.  

```csharp
public class Car
{
	const int DEFAULT_PETROL = 100;
	private int _petrol;

	// Default constructor
	public Car()
	{
		// They didn't pass a parameter, so assume defaults.
		_petrol = DEFAULT_PETROL;
	}

	public Car(int petrol)
	{
		_petrol = petrol;
	}
}
```  

An example of using these constructors:  

```csharp
// This will create a car, calling the default constructor.
Car myCar = new Car();
// And this uses the constructor with one argument.
Car myOtherCar = new Car(50);
```

## Interfaces
Interfaces are another core part of OOP in C#. An interface expresses a behaviour that a class may choose to implement. Unlike a derived class, any class may have as many interfaces as you like. For example, we could define a “IExplodable” interface:  

```csharp
public interface IExplodable
{
	void Explode();
}
```  

This defines an IExplodable interface, with a single method (Explode). Interfaces are always abstract; we can't create a direct instance of them. This means that we don't use the interface directly, instead we have classes that use the interfaces. For example, we could make our SportsCar IExplodable:  

```csharp
class SportsCar : Car, IExplodable
{
	// IExplodable implementation
	public void Explode()
	{
		// ...
	}
}
```  

This makes the SportsCar use the IExplodable interface.

We can treat anything that implements IExplodable in the same way (this is Polymorphism at work). For example, a function like this:  

```csharp
public void ExplodeIt(IExplodable thing)
{
	thing.Explode();
}
```  

We can pass this any object that implements IExplodable:  

```csharp
SportsCar myCar = new SportsCar();
Bike myBike = new Bike();
Microwave mike = new Microwave();
Potato awesomePotato = new Potato();
ExplodeIt(myCar);
ExplodeIt(myBike);
ExplodeIt(mike);
ExplodeIt(awesomePotato);
```

# Object design
Now that I've covered all the basics of object orientation, I thought I'd quickly mention some things about object design. There are several things that contribute to a good design:

* Don't put all the functionality in the one massive class or method (“centralised”). Instead, spread the functionality across multiple classes and multiple methods. This eases development, makes testing easier (see unit testing, above), and also makes the code more reusable.
* Excessive coupling is not good, because it prevents the reuse of objects in other projects. Coupling refers to the degree to which each object relies on one of the other objects. If the objects all rely on each other too much, then changes in one will require a whole heap of changes in others, and they're not as reusable.
* High cohesion is good. Cohesion refers to how strongly-related or focused the responsibilities of a single class are. If all the methods of the class tend to be similar, the class is said to have high cohesion. High cohesion leads to more readable code, and the likelihood of code reuse inceases. As an example, in the Car class, the methods (eg. StartCar, Drive, etc) all do something to the cat.

# Culture
Talking about programming culture usually involves discussion of coding standards. Coding standards are important because we need to stay consistent throughout our code, so other developers are able to understand it. The main standards that are followed in C# (including those [followed at Swinburne](http://mercury.it.swin.edu.au/swinbrain/index.php/Swinburne_.NET_Coding_Standard)) include:

* Use an underscore at the start of private members of a class (e.g. _petrol in the examples above)
* Use camel casing for local variables and parameters. Camel Case is basically words stuck together, with each word except the first starting with a capital letter (e.g. myCar)
* Use pascal casing (like camel case, except with the first letter capitalised as well) for the names of Classes and Methods (e.g. Car, SportsCar)
* Use an I to prefix the names of interfaces (e.g. IExplodable)
* File names should be the same as the class name (e.g. Car.cs, SportsCar.cs)
* Use meaningful descriptive words for the names of variables, classes, methods, objects, etc. The names should be useful so we know what the class or method is without looking through all the code.

Well, that's all for now. Hope this has helped someone :)

— Daniel

