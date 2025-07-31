#----------------------------------------------------------------
# Generated CMake target import file for configuration "Debug".
#----------------------------------------------------------------

# Commands may need to know the format version.
set(CMAKE_IMPORT_FILE_VERSION 1)

# Import target "libversion::libversion" for configuration "Debug"
set_property(TARGET libversion::libversion APPEND PROPERTY IMPORTED_CONFIGURATIONS DEBUG)
set_target_properties(libversion::libversion PROPERTIES
  IMPORTED_IMPLIB_DEBUG "${_IMPORT_PREFIX}/lib/libversion.lib"
  IMPORTED_LOCATION_DEBUG "${_IMPORT_PREFIX}/bin/libversion.dll"
  )

list(APPEND _cmake_import_check_targets libversion::libversion )
list(APPEND _cmake_import_check_files_for_libversion::libversion "${_IMPORT_PREFIX}/lib/libversion.lib" "${_IMPORT_PREFIX}/bin/libversion.dll" )

# Import target "libversion::libversion_static" for configuration "Debug"
set_property(TARGET libversion::libversion_static APPEND PROPERTY IMPORTED_CONFIGURATIONS DEBUG)
set_target_properties(libversion::libversion_static PROPERTIES
  IMPORTED_LINK_INTERFACE_LANGUAGES_DEBUG "C"
  IMPORTED_LOCATION_DEBUG "${_IMPORT_PREFIX}/lib/version.lib"
  )

list(APPEND _cmake_import_check_targets libversion::libversion_static )
list(APPEND _cmake_import_check_files_for_libversion::libversion_static "${_IMPORT_PREFIX}/lib/version.lib" )

# Commands beyond this point should not need to know the version.
set(CMAKE_IMPORT_FILE_VERSION)
