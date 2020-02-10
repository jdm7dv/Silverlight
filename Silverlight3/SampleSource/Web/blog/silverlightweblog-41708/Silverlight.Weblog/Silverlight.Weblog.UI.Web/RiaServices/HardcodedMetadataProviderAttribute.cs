//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Web.DomainServices;
//using Silverlight.Weblog.Shared.Common.Web.Helpers;

//namespace Silverlight.Weblog.UI.Web.RiaServices
//{
////    [HardcodedMetadataProviderAttribute(typeof(BlogDomainService), "SetMetadata")]



        ////public static void SetMetadata()
        ////{
        ////    HardcodedMetadataProviderAttribute.HardcodedMetadata.AddKey(typeof(BlogPost));
        ////    HardcodedMetadataProviderAttribute.HardcodedMetadata.AddOneToOne((BlogPost b) => b.User);
        ////    HardcodedMetadataProviderAttribute.HardcodedMetadata.AddOneToMany((Comment c) => c.BlogPost, (BlogPost b) => b.Comments);
        ////}


//// Copied from: http://www.nikhilk.net/RIA-Services-Fluent-Metadata-API.aspx
//    public class HardcodedMetadataProviderAttribute : MetadataProviderAttribute
//    {
//        public HardcodedMetadataProviderAttribute(Type typeToInvoke, string methodToInvoke)
//            : base(typeof(HardcodedMetadata))
//        {
//            this.typeToInvoke = typeToInvoke;
//            this.methodToInvoke = methodToInvoke;
//        }

//        Type typeToInvoke { get; set; }
//        string methodToInvoke { get; set; }

//        public override TypeDescriptionProvider CreateProvider(TypeDescriptionProvider existingProvider, Type domainServiceType)
//        {
//            typeToInvoke.GetMethod(methodToInvoke, BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[0]);
//            return new HardcodedMetadata();
//        }

//        public class HardcodedMetadata : TypeDescriptionProvider
//        {
//            private static List<TypePropertyAttribute> Attributes = new List<TypePropertyAttribute>();


//            public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
//            {
//                ICustomTypeDescriptor baseDescriptor = base.GetTypeDescriptor(objectType, instance);
//                return new HardcodedCustomTypeDescriptor(baseDescriptor, Attributes.Where(a => a.Type == objectType)); ;
//            }

//            public static void AddKey(Type type)
//            {
//                Attributes.Add(new TypePropertyAttribute(type,
//                                                         "ID",
//                                                         new KeyAttribute()));
//            }

//            public static void AddOneToOne<T, TReturn>(Expression<Func<T, TReturn>> property)
//            {
//                Attributes.Add(new TypePropertyAttribute(typeof(T),
//                                                         property.GetPropertyName(),
//                                                         new IncludeAttribute()));
//            }

//            public static void AddOneToMany<TOne, TOneType, TMany, TManyType>(Expression<Func<TOne, TOneType>> OneProperty
//                                                                              , Expression<Func<TMany, TManyType>> ManyProperty)
//            {
//                Attributes.Add(new TypePropertyAttribute(typeof(TOne),
//                                                         OneProperty.GetPropertyName(),
//                                                         new IncludeAttribute()));

//                Attributes.Add(new TypePropertyAttribute(typeof(TMany),
//                                                         ManyProperty.GetPropertyName(),
//                                                         new IncludeAttribute()));


//                Attributes.Add(new TypePropertyAttribute(
//                                   typeof(TMany),
//                                   ManyProperty.GetPropertyName(),
//                                   new AssociationAttribute(
//                                       string.Format("{0}_{1}_{2}_{3}", typeof(TOne).Name, OneProperty.GetPropertyName(), typeof(TMany).Name, ManyProperty.GetPropertyName()),
//                                       OneProperty.GetPropertyName(),
//                                       ManyProperty.GetPropertyName())));
//            }


//        }


//        internal class TypePropertyAttribute
//        {
//            public TypePropertyAttribute(Type type, string propertyName, Attribute attribute)
//            {
//                Type = type;
//                PropertyName = propertyName;
//                Attribute = attribute;
//            }

//            public Type Type { get; set; }
//            public string PropertyName { get; set; }
//            public Attribute Attribute { get; set; }
//        }

//        public class HardcodedCustomTypeDescriptor : CustomTypeDescriptor
//        {
//            internal HardcodedCustomTypeDescriptor(ICustomTypeDescriptor baseDescriptor, IEnumerable<TypePropertyAttribute> attributes)
//                : base(baseDescriptor)
//            {
//                this.customAttributes = attributes;
//            }

//            IEnumerable<TypePropertyAttribute> customAttributes { get; set; }


//            public override PropertyDescriptorCollection GetProperties()
//            {
//                PropertyDescriptorCollection propDescs = base.GetProperties();

//                List<PropertyDescriptor> newPropDescs = new List<PropertyDescriptor>(propDescs.Count);

//                foreach (PropertyDescriptor propDesc in propDescs)
//                {
//                    List<Attribute> memberAttributes = customAttributes.Where(c => c.PropertyName == propDesc.Name).Select(c => c.Attribute).ToList();
//                    if (memberAttributes == null)
//                    {
//                        newPropDescs.Add(propDesc);
//                    }
//                    else
//                    {
//                        newPropDescs.Add(new WrappedPropertyDescriptor(propDesc, memberAttributes.ToArray()));
//                    }
//                }


//                return propDescs;
//            }
//        }

//        private sealed class WrappedPropertyDescriptor : PropertyDescriptor
//        {

//            private PropertyDescriptor _parentPropDesc;

//            public WrappedPropertyDescriptor(PropertyDescriptor propDesc, Attribute[] attributes)
//                : base(propDesc, attributes)
//            {
//                _parentPropDesc = propDesc;
//            }

//            public override Type ComponentType
//            {
//                get
//                {
//                    return _parentPropDesc.ComponentType;
//                }
//            }

//            public override bool IsReadOnly
//            {
//                get
//                {
//                    return _parentPropDesc.IsReadOnly;
//                }
//            }

//            public override Type PropertyType
//            {
//                get
//                {
//                    return _parentPropDesc.PropertyType;
//                }
//            }

//            public override bool CanResetValue(object component)
//            {
//                return _parentPropDesc.CanResetValue(component);
//            }

//            public override object GetValue(object component)
//            {
//                return _parentPropDesc.GetValue(component);
//            }

//            public override void ResetValue(object component)
//            {
//                _parentPropDesc.ResetValue(component);
//            }

//            public override void SetValue(object component, object value)
//            {
//                _parentPropDesc.SetValue(component, value);
//            }

//            public override bool ShouldSerializeValue(object component)
//            {
//                return _parentPropDesc.ShouldSerializeValue(component);

//            }
//        }
//    }
//}