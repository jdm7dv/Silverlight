using System;
using System.Collections;
using Microsoft.Windows.Controls;

namespace SampleTheme
{
    /// <summary>
    /// Fruit business object used for theme sample.
    /// </summary>
    public class Fruit
    {
        /// <summary>
        /// Gets or sets the name of the fruit.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price of the Fruit.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Initializes a new instance of the City class.
        /// </summary>
        public Fruit()
        {
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Gets a collection of fruits.
        /// </summary>
        public static ObjectCollection FruitsSample
        {
            get
            {
                ObjectCollection fruits = new ObjectCollection();
                fruits.Add(new Fruit { Name = "Banana", Price = 1 });
                fruits.Add(new Fruit { Name = "Strawberry", Price = 4 });
                fruits.Add(new Fruit { Name = "Watermelon", Price = 7 });
                return fruits;
            }
        }
    }
}