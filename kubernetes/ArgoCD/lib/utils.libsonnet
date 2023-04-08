{
  toEnv(obj)::
    local toEnvInternal(obj, prefix) =
      std.filter(function(v) v != null,
        [
          if !std.isObject(obj[key]) then
            { name: prefix + key, value: obj[key] }
          for key in std.objectFields(obj)
        ] 
        + std.flattenArrays(std.filter(function(v) v != null, [
          if std.isObject(obj[key]) then
            toEnvInternal(obj[key], prefix + key + "__")
          for key in std.objectFields(obj)
        ])));

    toEnvInternal(obj, "")
}