Summary for builder pattern: 
* A builder is separate component for building an object
* Can either give builder a constructor or return it via a static function
* To make builder fluent (methods that can chain each other), return the builder itself (using "this" keyword)
* Different facets of an object can be built with different builders working in tandem via base class