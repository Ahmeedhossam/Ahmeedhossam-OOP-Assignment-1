# Part 3 — Builder Pattern Answers

## Task 3.1 — Problems with a 20-Parameter Constructor

### 1. Why is a single 20-parameter constructor a problem in practice?

* **Hard to Read When Calling the Constructor:** Passing 20 values in one line makes the code very hard to read. When someone looks at raw values like `true`, `false`, numbers, or long lists of strings, they cannot tell what each value means without opening the class file to check the parameter names.
* **Easy to Swap Values of the Same Type by Mistake:** When many parameters next to each other have the same data type (like several `string` fields for billing and shipping addresses, or several `decimal` fields like `SubTotal`, `DiscountAmount`, and `TaxAmount`), it is very easy to pass them in the wrong order. Because the types match, the C# compiler will not show any error, which leads to silent bugs and wrong calculations at runtime.
* **Forcing Callers to Pass Unneeded Values:** Even when some properties are optional (like a `0` discount or optional notes), the caller is still forced to pass default or dummy values for all of them just to reach the rest of the parameters.
* **Hard to Update Later (Breaking Changes):** If we need to add just one new optional property in the future, we have to change the constructor. This breaks every place in the project that calls it, or forces us to create many confusing constructor overloads.

---

### 2. Is this purely a "constructor is too long" problem, or is there a deeper design issue?

It is a deeper object-oriented design problem, not just a problem with the constructor's length:

* **Too Many Responsibilities in One Class (Low Cohesion):** Putting ~20 loosely related properties into one flat class makes it a "God Class" that holds too many different things at once: customer details, two addresses, and payment/order numbers. It does not follow the Single Responsibility Principle.
* **Repeated Fields Instead of Separate Classes:** Writing the exact same five address fields twice (`BillingStreet`, `BillingCity`, etc., and `ShippingStreet`, `ShippingCity`, etc.) as plain `string` variables shows a lack of proper OOP modeling. Instead of keeping everything flat as primitives, related fields should be grouped into their own classes (like an `Address` class and an `OrderDetails` class) so their data and validation rules are written once and reused cleanly.