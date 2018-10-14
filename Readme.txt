Author: Ronald Norman
Date: October 9, 2018

Main Functionality
=============================================
This solution is for a themometer that allows subscribers to listen in for temperature change events. These events could be either temperature dropping or temperature rising events. In addition, it allows a certain sensitivity variance that the subscriber can set to limit the number of notifications received.

Quality Attributes (Non-Functional)
=============================================
This solution demonstrates the use events to decouple components, and also the use of design patterns such as the Template Method to eliminate redundancy in design, OO principles such as encapsulation and the use of interfaces, not violating the DRY principle, and a little bit of DDD by using an Aggregate domain object as the entry point. 

It is also extensible as it makes it simple to add more domain events or temperature scales.

It is maintainable and well commented for ease of readability with the use of descriptive naming.

The solution is also testable as I've demonstrated the use of data source unit tests using MSTest.

Functionality
=============================================
1. Receive temperature drop or rise events
2. Ability to add a sensitivity variance to limit notifications
3. Displays temperature in either Fahrenheit or Celsius
4. Support both Fahrenheit or Celsius input
5. Supports multiple temperature drop AND rise subscribers per thermometer

Assumptions
============================================
1. The input is simply a SetReading() or GetReading(), ideally, it sould be reading from a sensor
2. Assumes that this is a library that will be used by other components
