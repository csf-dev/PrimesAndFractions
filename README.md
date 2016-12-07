# CSF Utilities
This *general purpose utilities library* is an assortment of helper types which
are too small to warrant their own assemblies. It's where I am putting
specks of useful functionality for which I can't find a better home.

## Highlights
Here are some of the highlights of functionality which is contained within:

* **`OrderNeutralEqualityComparer<T>`** - an `IEqualityComparer` which compares collections for equivalence
* **`BinaryGuidCombStrategy`** - creates GUIDs using [the COMB strategy]
* **Extension methods for enums**  - particularly useful for those decorated `[Flags]`
* **`Fraction`** - a `struct` for representing rational numbers in the format "x/y"
* **`PrimeFactoriser`** - a helper for generating prime numbers/prime factors, [based on] this [excellent work]

[the COMB strategy]: http://www.informit.com/articles/article.aspx?p=25862
[based on]: http://handcraftsman.wordpress.com/2010/09/02/ienumerable-of-prime-numbers-in-csharp/
[excellent work]: http://handcraftsman.wordpress.com/2010/09/02/prime-factorization-in-csharp/

## Open source license
All source files within this project are released as open source software,
under the terms of [the MIT license].

[the MIT license]: http://opensource.org/licenses/MIT

This software is distributed in the hope that it will be useful, but please
remember that:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
