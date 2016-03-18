// additional metadata applied to crate, module or item
// crate: #![...]
// other: #[...]

// conditional compilation
// crate information
// disable lints
// enable additional compiler features
// link to a foreign library
// mark function as unit test
// mark function that will be part of a benchmark

// #[attribute = "value"]
// #[attribute(key = value)]
// #[attribute(value)]

// условная компиляция:
// #[cfg(target_os = "linux")]
// #[cfg(not(target_os = "linux"))]
// #[cfg(any(unix, windows))]
// #[cfg(all(unix, target_pointer_width = "32"))]
// cfg!(...) для логических выражений
// дополнительные (пользовательские) ключи мб указаны переданы компилятору:переданы
// rustc --cfg some_condition custom.rs
// или указаны в секции features для cargo:
// [features]
// feature = ["feature_name"]
// это преобразуется в 
// --cfg feature="${feature_name}"
// при компиляции
