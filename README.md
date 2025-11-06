# Cirreum.ExpressionBuilder

[![NuGet Version](https://img.shields.io/nuget/v/Cirreum.ExpressionBuilder.svg?style=flat-square)](https://www.nuget.org/packages/Cirreum.ExpressionBuilder/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Cirreum.ExpressionBuilder.svg?style=flat-square)](https://www.nuget.org/packages/Cirreum.ExpressionBuilder/)
[![GitHub Release](https://img.shields.io/github/v/release/cirreum/Cirreum.ExpressionBuilder?style=flat-square)](https://github.com/cirreum/Cirreum.ExpressionBuilder/releases)

A powerful, fluent C# library for dynamically building LINQ expressions and lambda functions. Build complex queries programmatically with compile-time safety and runtime flexibility.

## Features

- **Fluent API**: Intuitive, chainable syntax for building complex filters
- **Type Safety**: Compile-time checking with generic type constraints
- **Dynamic Querying**: Build expressions from user input, configuration, or runtime conditions
- **Null Safety**: Automatic null-checking prevents runtime exceptions
- **Collection Support**: Query nested collections and complex object graphs
- **Extensible**: Create custom operators for domain-specific filtering
- **Database Ready**: Works seamlessly with Entity Framework and other ORMs
- **Client-Side Ready**: Perfect for Blazor and other client-side scenarios where dynamic filtering needs to be built from user input

## Quick Start

### Installation

```bash
dotnet add package Cirreum.ExpressionBuilder
```

### Basic Usage

```csharp
using Cirreum.ExpressionBuilder;

// Simple property filtering
var filter = new Filter<Person>();
filter.By("Age", Operators.GreaterThan, 18)
      .And.By("Name", Operators.Contains, "John");

var adults = people.Where(filter).ToList();
```

### Complex Queries

```csharp
// Grouped conditions with nested properties
var filter = new Filter<Person>();
filter.By("Age", Operators.Between, 18, 65)
      .And.Group
          .By("Department.Name", Operators.EqualTo, "Engineering")
          .Or.By("Department.Name", Operators.EqualTo, "Product")
      .And.By("Contacts[Type]", Operators.EqualTo, ContactType.Email);

var results = employees.Where(filter).ToList();
```

## Property Conventions

The library uses a simple convention for referencing properties:

| Pattern | Example | Description |
|---------|---------|-------------|
| `PropertyName` | `"Age"` | Direct property access |
| `Parent.Property` | `"Department.Name"` | Nested object properties |
| `Collection[Property]` | `"Contacts[Email]"` | Properties of collection items |

## Supported Operations

### Text Operations

- `Contains`, `DoesNotContain`
- `StartsWith`, `EndsWith`
- `EqualTo`, `NotEqualTo`
- `IsEmpty`, `IsNotEmpty`
- `IsNull`, `IsNotNull`
- `IsNullOrWhiteSpace`, `IsNotNullNorWhiteSpace`

### Numeric/Date Operations

- `EqualTo`, `NotEqualTo`
- `GreaterThan`, `GreaterThanOrEqualTo`
- `LessThan`, `LessThanOrEqualTo`
- `Between`

### Collection Operations

- `In`, `NotIn` (check if value exists in a list)

### Boolean Operations

- `EqualTo`, `NotEqualTo`

## Advanced Examples

### Dynamic Filtering from User Input

```csharp
public IQueryable<Product> FilterProducts(IQueryable<Product> products, 
                                        FilterRequest request)
{
    var filter = new Filter<Product>();
    
    if (!string.IsNullOrEmpty(request.Category))
        filter.By("Category", Operators.EqualTo, request.Category);
    
    if (request.MinPrice.HasValue)
        filter.And.By("Price", Operators.GreaterThanOrEqualTo, request.MinPrice.Value);
    
    if (request.MaxPrice.HasValue)
        filter.And.By("Price", Operators.LessThanOrEqualTo, request.MaxPrice.Value);
    
    return products.Where(filter);
}
```

### Grouping Conditions

```csharp
var filter = new Filter<Employee>();

// (Department = 'IT' OR Department = 'Engineering') 
// AND (Status = 'Active' AND HireDate > '2020-01-01')
filter.By("Department", Operators.EqualTo, "IT")
      .Or.By("Department", Operators.EqualTo, "Engineering")
      .And.Group
          .By("Status", Operators.EqualTo, "Active")
          .And.By("HireDate", Operators.GreaterThan, new DateTime(2020, 1, 1));

var results = employees.Where(filter).ToList();
```

### Custom Operators

Create domain-specific operators for specialized filtering:

```csharp
public class IsWithinRadius : OperatorBase
{
    public IsWithinRadius() : base("IsWithinRadius", 2, TypeGroup.Number) { }
    
    public override Expression GetExpression(MemberExpression member, 
                                           ConstantExpression lat, 
                                           ConstantExpression lng)
    {
        // Implement distance calculation logic
        // Return expression for: distance <= radius
    }
}

// Register custom operator
Operators.LoadOperators(new List<IOperator> { new IsWithinRadius() });
```

## Entity Framework Integration

Works seamlessly with EF Core for database queries:

```csharp
public async Task<List<Customer>> GetFilteredCustomersAsync(CustomerFilter filterRequest)
{
    var filter = new Filter<Customer>();
    
    if (!string.IsNullOrEmpty(filterRequest.City))
        filter.By("Address.City", Operators.EqualTo, filterRequest.City);
    
    if (filterRequest.HasActiveOrders)
        filter.And.By("Orders[Status]", Operators.EqualTo, OrderStatus.Active);
    
    return await context.Customers
        .Where(filter)
        .ToListAsync();
}
```

## Performance Considerations

- Expressions are compiled once and can be reused
- Use `IQueryable` interfaces for database queries to ensure server-side execution
- Complex nested queries may benefit from database indexes on filtered properties

## Error Handling

The library provides detailed exceptions for common issues:

- `UnsupportedOperationException`: Operation not supported for the property type
- `PropertyValueTypeMismatchException`: Type mismatch between property and filter value
- `WrongNumberOfValuesException`: Incirreumect number of values for an operation

## Thread Safety

`Filter<T>` instances are not thread-safe. Create separate instances for concurrent operations or use appropriate synchronization.

## Acknowledgments

This library builds upon the excellent foundation provided by [David Belmont's ExpressionBuilder](https://github.com/dbelmont/ExpressionBuilder). We've extended and modernized the codebase while maintaining the core philosophy of simple, powerful expression building.

## Changelog

### v1.0

- Current stable version
- Versioning follows the Cirreum Framework SemVer version
