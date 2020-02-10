// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.Windows.Media.Effects;
using System.Windows;
using System.Windows.Media;

namespace MagicMirror.ShaderEffectLibrary
{
  public class SwirlEffect : ShaderEffect
  {
    private static PixelShader _pixelShader = new PixelShader()
    {
      UriSource = new Uri("/MagicMirror;component/Shaders/Swirl.ps", UriKind.Relative)
    };

    public SwirlEffect()
    {
      PixelShader = _pixelShader;

      UpdateShaderValue(InputProperty);
      UpdateShaderValue(FactorProperty);
    }

    public static readonly DependencyProperty InputProperty =
      ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(SwirlEffect), 0);

    public Brush Input
    {
      get { return (Brush)GetValue(InputProperty); }
      set { SetValue(InputProperty, value); }
    }

    public static readonly DependencyProperty FactorProperty =
      DependencyProperty.Register("Factor",
        typeof(double), typeof(SwirlEffect), new PropertyMetadata(0.0, PixelShaderConstantCallback(0)));

    public double Factor
    {
      get { return (double)GetValue(FactorProperty); }
      set { SetValue(FactorProperty, value); }
    }
  }
}