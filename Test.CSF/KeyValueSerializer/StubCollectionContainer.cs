using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.CSF.KeyValueSerializer
{
  /// <summary>
  /// A stub type imported from another (closed source) project that I am working on, anonymised.  This is designed to
  /// test for a specific issue when using the serializer in that project.
  /// </summary>
  public class StubCollectionContainer
  {
    #region fields

    private ICollection<DateTime> _addColumns;
    private ICollection<DateTime> _removeColumns;
    private ICollection<DateTime> _moveColumns;
    private ICollection<DateTime> _mandatoryColumns;

    #endregion

    #region properties

    public virtual ICollection<DateTime> AddColumns
    {
      get {
        return _addColumns;
      }
      set {
        _addColumns = value?? new List<DateTime>();
      }
    }

    public virtual ICollection<DateTime> RemoveColumns
    {
      get {
        return _removeColumns;
      }
      set {
        _removeColumns = value?? new List<DateTime>();
      }
    }

    public virtual ICollection<DateTime> MoveColumns
    {
      get {
        return _moveColumns;
      }
      set {
        _moveColumns = value?? new List<DateTime>();
      }
    }

    public virtual ICollection<DateTime> MandatoryColumns
    {
      get {
        return _mandatoryColumns;
      }
      set {
        _mandatoryColumns = value?? new List<DateTime>();
      }
    }

    #endregion

    #region constructor

    public StubCollectionContainer ()
    {
      this.AddColumns = new List<DateTime>();
      this.RemoveColumns = new List<DateTime>();
      this.MoveColumns = new List<DateTime>();
      this.MandatoryColumns = new List<DateTime>();
    }

    #endregion
  }
}

