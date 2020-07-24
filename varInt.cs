namespace ASEProject
{
    /// <summary>
    ///   <para>User declared vars</para>
    ///   <para></para>
    /// </summary>
    class VarInt
    {
        /// <summary>The name</summary>
        public string name;
        /// <summary>The value</summary>
        public int value;

        /// <summary>Initializes a new instance of the <see cref="VarInt"/> class.</summary>
        /// <param name="name_">The name.</param>
        /// <param name="value_">The value.</param>
        public VarInt(string name_, int value_)
        {
            name = name_;
            value = value_;
        }
        /// <summary>Sets the int.</summary>
        /// <param name="toSet">To set.</param>
        public void setInt(int toSet)
        {
            value = toSet;
        }
        /// <summary>Gets the value.</summary>
        /// <returns></returns>
        public int getValue()
        {
            return value;
        }

        /// <summary>Gets the name.</summary>
        /// <returns></returns>
        public string getName()
        {
            return name;
        }

    }
}
 
