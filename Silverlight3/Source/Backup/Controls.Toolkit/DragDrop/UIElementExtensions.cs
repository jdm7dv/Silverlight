﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
#if SILVERLIGHT
using SW = Microsoft.Windows;
#else
using SW = System.Windows;
#endif

namespace System.Windows
{
    /// <summary>
    /// A collection of extension methods for the UIElement class.
    /// </summary>
    /// <QualityBand>Experimental</QualityBand>
    internal static partial class UIElementExtensions
    {
        /// <summary>
        /// Returns an observable that wraps the MouseMove event.
        /// </summary>
        /// <param name="that">The instance to retrieve the event for.</param>
        /// <returns>An observable that wraps the MouseMove event.</returns>	
        internal static IObservable<Event<MouseEventArgs>> GetMouseMove(this UIElement that)
        {
            return new AnonymousObservable<Event<MouseEventArgs>>(
                observer =>
                {
                    MouseEventHandler handler = (o, a) => observer.OnNext(new Event<MouseEventArgs>(o, a));
                    that.MouseMove += handler;
                    return new AnonymousDisposable(() => that.MouseMove -= handler);
                });
        }

        /// <summary>
        /// Returns an observable that wraps the MouseMove event on an instance 
        /// and all of it's siblings.
        /// </summary>
        /// <param name="that">The instance to retrieve the event for.</param>
        /// <returns>An observable that wraps the MouseMove event on an instance 
        /// and all of it's siblings.</returns>
        internal static IObservable<Event<MouseEventArgs>> GetMouseMoveOnSelfAndSiblings(this UIElement that)
        {
            IEnumerable<UIElement> popupRoots =
                that
                    .GetVisualDescendants()
                    .OfType<Popup>()
                    .Select(popup => popup.Child)
                    .Where(popupRoot => popupRoot != null);

            return
                popupRoots
                    .Select(popupRoot => popupRoot.GetMouseMove())
                    .Aggregate(that.GetMouseMove(), (left, right) => left.Merge(right));
        }

        /// <summary>
        /// Returns an observable that wraps the MouseLeftButtonDown event and
        /// returns even if one of the events involved is handled.
        /// </summary>
        /// <param name="that">The element to listen to.</param>
        /// <returns>An observable that that wraps the MouseLeftButtonDown event 
        /// and returns even if one of the events involved is handled.</returns>
        internal static IObservable<Event<MouseButtonEventArgs>> GetMouseLeftButtonDownAlways(this UIElement that)
        {
            return new AnonymousObservable<Event<MouseButtonEventArgs>>(
                (observer) =>
                {
                    MouseButtonEventHandler handler = (o, a) => observer.OnNext(new Event<MouseButtonEventArgs>(o, a));
                    that.AddHandler(UIElement.MouseLeftButtonDownEvent, handler, true);

                    return new AnonymousDisposable(() => that.RemoveHandler(UIElement.MouseLeftButtonDownEvent, handler));
                });
        }

        /// <summary>
        /// Returns an observable that wraps the MouseLeftButtonUp event of the
        /// root visual and all of it's siblings and returns even if one of the 
        /// events involved is handled.
        /// </summary>
        /// <param name="that">The element to listen to.</param>
        /// <returns>An observable that that wraps the MouseLeftButtonUp event 
        /// and returns even if one of the events involved is handled.</returns>
        internal static IObservable<Event<MouseButtonEventArgs>> GetMouseLeftButtonUpOnSelfAndSiblingsAlways(this UIElement that)
        {
            IEnumerable<UIElement> popupRoots =
                            VisualTreeExtensions.GetVisualDescendants(that)
                                .OfType<Popup>()
                                .Select(popup => popup.Child)
                                .Where(popupRoot => popupRoot != null);

            return
                popupRoots
                    .Select(popupRoot => popupRoot.GetMouseLeftButtonUpAlways())
                    .Aggregate(that.GetMouseLeftButtonUpAlways(), (left, right) => left.Merge(right));
        }

        /// <summary>
        /// Returns an observable that wraps the MouseLeftButtonUp event and
        /// returns even if one of the events involved is handled.
        /// </summary>
        /// <param name="that">The element to listen to.</param>
        /// <returns>An observable that that wraps the MouseLeftButtonUp event 
        /// and returns even if one of the events involved is handled.</returns>
        internal static IObservable<Event<MouseButtonEventArgs>> GetMouseLeftButtonUpAlways(this UIElement that)
        {
            return new AnonymousObservable<Event<MouseButtonEventArgs>>(
                (observer) =>
                {
                    MouseButtonEventHandler handler = (o, a) => observer.OnNext(new Event<MouseButtonEventArgs>(o, a));
                    that.AddHandler(UIElement.MouseLeftButtonUpEvent, handler, true);

                    return new AnonymousDisposable(() => that.RemoveHandler(UIElement.MouseLeftButtonUpEvent, handler));
                });
        }

        /// <summary>
        /// Returns an observable that wraps the KeyDown event and
        /// returns even if one of the events involved is handled.
        /// </summary>
        /// <param name="that">The element to listen to.</param>
        /// <returns>An observable that that wraps the KeyDown event 
        /// and returns even if one of the events involved is handled.</returns>
        internal static IObservable<Event<KeyEventArgs>> GetKeyDownAlways(this UIElement that)
        {
            return new AnonymousObservable<Event<KeyEventArgs>>(
                (observer) =>
                {
                    KeyEventHandler handler = (o, a) => observer.OnNext(new Event<KeyEventArgs>(o, a));
                    that.AddHandler(UIElement.KeyDownEvent, handler, true);

                    return new AnonymousDisposable(() => that.RemoveHandler(UIElement.KeyDownEvent, handler));
                });
        }

        /// <summary>
        /// Returns an observable that wraps the KeyUp event and
        /// returns even if one of the events involved is handled.
        /// </summary>
        /// <param name="that">The element to listen to.</param>
        /// <returns>An observable that that wraps the KeyUp event 
        /// and returns even if one of the events involved is handled.</returns>
        internal static IObservable<Event<KeyEventArgs>> GetKeyUpAlways(this UIElement that)
        {
            return new AnonymousObservable<Event<KeyEventArgs>>(
                (observer) =>
                {
                    KeyEventHandler handler = (o, a) => observer.OnNext(new Event<KeyEventArgs>(o, a));
                    that.AddHandler(UIElement.KeyUpEvent, handler, true);

                    return new AnonymousDisposable(() => that.RemoveHandler(UIElement.KeyUpEvent, handler));
                });
        }

        /// <summary>
        /// Returns an observable that wraps the KeyUp event and returns even if 
        /// one of the events involved is handled or occurs in a sibling.
        /// </summary>
        /// <param name="that">The element to listen to.</param>
        /// <returns>An observable that wraps the KeyUp event and returns even if 
        /// one of the events involved is handled or occurs in a sibling.</returns>
        internal static IObservable<Event<KeyEventArgs>> GetKeyUpOnSelfAndSiblingsAlways(this UIElement that)
        {
            IEnumerable<UIElement> popupRoots =
                VisualTreeExtensions.GetVisualDescendants(that)
                    .OfType<Popup>()
                    .Select(popup => popup.Child)
                    .Where(popupRoot => popupRoot != null);

            return
                popupRoots
                    .Select(popupRoot => popupRoot.GetKeyUpAlways())
                    .Aggregate(that.GetKeyUpAlways(), (left, right) => left.Merge(right));
        }

        /// <summary>
        /// Returns an observable that returns a SW.DragDropKeyStates value.  The
        /// observable is composed of mouse down and up observables and key down
        /// and up observables.
        /// </summary>
        /// <param name="mouseDownObservable">An event raised when a mouse 
        /// button is depressed.</param>
        /// <param name="mouseUpObservable">An event raised when a mouse button
        /// is released.</param>
        /// <param name="keyDownObservable">An event raised when a key is
        /// pressed down.</param> 
        /// <param name="keyUpObservable">An event raised when a key is 
        /// released.</param>
        /// <param name="initialState">The initial state of the drag and
        /// drop keys.</param>
        /// <returns>An observable that returns a SW.DragDropKeyStates value
        /// whenever it changes, even if one of the events involved
        /// is handled.</returns>
        private static IObservable<SW.DragDropKeyStates> GetKeyStateChanged(
            IObservable<Event<MouseButtonEventArgs>> mouseDownObservable, 
            IObservable<Event<MouseButtonEventArgs>> mouseUpObservable, 
            IObservable<Event<KeyEventArgs>> keyDownObservable, 
            IObservable<Event<KeyEventArgs>> keyUpObservable,
            SW.DragDropKeyStates initialState)
        {
            return
                mouseDownObservable
                    .Select(ev => Tuple.Create(true, SW.DragDropKeyStates.LeftMouseButton))
                    .Merge(
                        mouseUpObservable
                            .Select(ev => Tuple.Create(false, SW.DragDropKeyStates.LeftMouseButton)))
                    .Merge(
                            keyUpObservable
                            .Select(ev => Tuple.Create(false, ToDragDropKeyStates(ev.EventArgs.Key))))
                    .Merge(
                            keyDownObservable
                            .Select(ev => Tuple.Create(true, ToDragDropKeyStates(ev.EventArgs.Key))))
                    .Scan<Tuple<bool, SW.DragDropKeyStates>, SW.DragDropKeyStates>(
                        initialState,
                        (acc, current) =>
                        {
                            if (current.Item1)
                            {
                                return acc | current.Item2;
                            }
                            else
                            {
                                return acc & ~current.Item2;
                            }
                        });
        }

        /// <summary>
        /// Returns an observable that returns a SW.DragDropKeyStates value
        /// whenever it changes, even if one of the events involved
        /// is handled.
        /// </summary>
        /// <param name="that">The element to listen to.</param>
        /// <param name="initialState">The initial state SW.DragDropKeyStates.
        /// </param>
        /// <returns>An observable that returns a SW.DragDropKeyStates value
        /// whenever it changes, even if one of the events involved
        /// is handled.</returns>
        internal static IObservable<SW.DragDropKeyStates> GetKeyStateChangedAlways(
            this UIElement that,
            SW.DragDropKeyStates initialState)
        {
            return
                GetKeyStateChanged(that.GetMouseLeftButtonDownAlways(), that.GetMouseLeftButtonUpAlways(), that.GetKeyDownAlways(), that.GetKeyUpAlways(), initialState);
        }

        /// <summary>
        /// Returns an observable that returns a SW.DragDropKeyStates value
        /// whenever it changes, even if one of the events involved
        /// is handled or occurs in a sibling.
        /// </summary>
        /// <param name="that">The element to listen to.</param>
        /// <param name="initialState">The initial state SW.DragDropKeyStates.
        /// </param>
        /// <returns>An observable that returns a SW.DragDropKeyStates value
        /// whenever it changes, even if one of the events involved
        /// is handled.</returns>
        internal static IObservable<SW.DragDropKeyStates> GetKeyStateChangedOnSelfAndSiblingsAlways(this UIElement that, SW.DragDropKeyStates initialState)
        {
            IEnumerable<UIElement> popupRoots =
                VisualTreeExtensions.GetVisualDescendants(that)
                    .OfType<Popup>()
                    .Select(popup => popup.Child)
                    .Where(popupRoot => popupRoot != null);

            return GetKeyStateChanged(
                popupRoots
                    .Select(popupRoot => popupRoot.GetMouseLeftButtonDownAlways())
                    .Aggregate(that.GetMouseLeftButtonDownAlways(), (left, right) => left.Merge(right)),
                popupRoots
                    .Select(popupRoot => popupRoot.GetMouseLeftButtonUpAlways())
                    .Aggregate(that.GetMouseLeftButtonUpAlways(), (left, right) => left.Merge(right)),
                popupRoots
                    .Select(popupRoot => popupRoot.GetKeyUpAlways())
                    .Aggregate(that.GetKeyUpAlways(), (left, right) => left.Merge(right)),
                popupRoots
                    .Select(popupRoot => popupRoot.GetKeyDownAlways())
                    .Aggregate(that.GetKeyDownAlways(), (left, right) => left.Merge(right)),
                initialState);                    
        }

        /// <summary>
        /// Returns an observable that returns a value indicating 
        /// whether escape is pressed, even if one of the events involved
        /// is handled or occurs in a sibling.
        /// </summary>
        /// <param name="that">The element to listen to.</param>
        /// <returns>An observable that returns a value indicating 
        /// whether escape is pressed, even if one of the events involved
        /// is handled or occurs in a sibling.</returns>
        internal static IObservable<bool> GetEscapePressedChangedOnSelfAndSiblingsAlways(this UIElement that)
        {
            return
                Observable.Merge(
                    that
                        .GetKeyUpOnSelfAndSiblingsAlways()
                        .Where(ev => ev.EventArgs.Key == Key.Escape)
                        .Select(ev => false),
                    that
                        .GetKeyUpOnSelfAndSiblingsAlways()
                        .Where(ev => ev.EventArgs.Key == Key.Escape)
                        .Select(ev => true));
        }

        /// <summary>
        /// Converts the key enumeration into the appropriate DragDropKeyStates
        /// value.
        /// </summary>
        /// <param name="key">The key value.</param>
        /// <returns>The appropriate SW.DragDropKeyStates value.</returns>
        private static SW.DragDropKeyStates ToDragDropKeyStates(Key key)
        {
            switch (key)
            {
                case Key.Ctrl:
                    return SW.DragDropKeyStates.ControlKey;
                case Key.Alt:
                    return SW.DragDropKeyStates.AltKey;
                case Key.Shift:
                    return SW.DragDropKeyStates.ShiftKey;
            }
            return SW.DragDropKeyStates.None;
        }

        /// <summary>
        /// This method performs a transform to visual operation but traps exceptions that occur.
        /// </summary>
        /// <param name="that">The element to transform to.</param>
        /// <param name="element">The element to transform from.</param>
        /// <returns>A general transform.</returns>
        internal static GeneralTransform SafeTransformToVisual(this UIElement that, UIElement element)
        {
            try
            {
                return that.TransformToVisual(element);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}