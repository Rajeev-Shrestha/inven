/// <binding BeforeBuild='get:started' />
var gulp = require('gulp');
var runSeq = require('run-sequence');
var del = require('del');

var buildConfig = require('./gulp.config');

//gulp.task('get:started', function(done) {
//    runSeq(
//       //'clean-Vendor-Js-In-Root',
//        'copy-Vendor-Js-To-Wwwroot-Internal',
//        done);
//});

//gulp.task('clean-Vendor-Js-In-Root', function() {
//    return del(buildConfig.rootJsFolder);
//});

//gulp.task('copy-Vendor-Js-To-Wwwroot-Internal', function(done) {
//  runSeq(
//        //'copy-angular',
//       // 'copy-rxjs',
//       'copy-allOther',
//        done);
//});

//gulp.task('copy-angular', function() {
//    return gulp.src(buildConfig.sources.angularRC)
//        .pipe(gulp.dest(buildConfig.rootJsFolder + "@angular/"));
//});

//gulp.task('copy-rxjs', function() {
//    return gulp.src(buildConfig.sources.Rxjs)
//        .pipe(gulp.dest(buildConfig.rootJsFolder + "rxjs/"));
//});

//gulp.task('copy-allOther', function() {
//    return gulp.src(buildConfig.sources.jsFilesInclSourcePaths)
//        .pipe(gulp.dest(buildConfig.rootJsFolder));
//});