const path = require('path');
const typescripPaths = require('./tsconfig.paths.json');

const trimPaths = (val) => val.slice(0, -2); // remove last '/*' chars
const convertArrayToObject = (array, keyFnc, valFnc) => {
  const initialValue = {};
  return array.reduce((obj, item) => {
    const key = keyFnc(item);
    const val = valFnc(item);
    return {
      ...obj,
      [key]: val,
    };
  }, initialValue);
};

module.exports = convertArrayToObject(Object
  .entries(typescripPaths.compilerOptions.paths),
    ([key, _value]) => trimPaths(key),
    ([_key, value]) => path.resolve(__dirname, `${typescripPaths.compilerOptions.baseUrl}/${trimPaths(value[0])}`),
  )
