//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

var MathHelperEpsilon = 0.000001;

/*
Want to try to abstract ourselves form the underlying
implementation of the algorithm

TODO: Implement faster versions if possible
*/

function MathHelperAreEqual(x,y)
{
    return MathHelperIsZero(x-y);
}

function MathHelperIsZero(x)
{
    return Math.abs(x) < MathHelperEpsilon;
}

function MathHelperSquareRoot(x)
{
    return Math.sqrt(x);
}

function MathHelperInverseSquareRoot(x)
{
    return 1.0 / Math.sqrt(x);
}