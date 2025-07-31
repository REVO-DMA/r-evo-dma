
#ifndef LIBVERSION_EXPORT_H
#define LIBVERSION_EXPORT_H

#ifdef LIBVERSION_STATIC_DEFINE
#  define LIBVERSION_EXPORT
#  define LIBVERSION_NO_EXPORT
#else
#  ifndef LIBVERSION_EXPORT
#    ifdef libversion_EXPORTS
        /* We are building this library */
#      define LIBVERSION_EXPORT __declspec(dllexport)
#    else
        /* We are using this library */
#      define LIBVERSION_EXPORT __declspec(dllimport)
#    endif
#  endif

#  ifndef LIBVERSION_NO_EXPORT
#    define LIBVERSION_NO_EXPORT 
#  endif
#endif

#ifndef LIBVERSION_DEPRECATED
#  define LIBVERSION_DEPRECATED __declspec(deprecated)
#endif

#ifndef LIBVERSION_DEPRECATED_EXPORT
#  define LIBVERSION_DEPRECATED_EXPORT LIBVERSION_EXPORT LIBVERSION_DEPRECATED
#endif

#ifndef LIBVERSION_DEPRECATED_NO_EXPORT
#  define LIBVERSION_DEPRECATED_NO_EXPORT LIBVERSION_NO_EXPORT LIBVERSION_DEPRECATED
#endif

#if 0 /* DEFINE_NO_DEPRECATED */
#  ifndef LIBVERSION_NO_DEPRECATED
#    define LIBVERSION_NO_DEPRECATED
#  endif
#endif

#endif /* LIBVERSION_EXPORT_H */
