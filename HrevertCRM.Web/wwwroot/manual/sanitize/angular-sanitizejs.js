﻿/*
 AngularJS v1.6.6-build.5421+sha.77b302a
 (c) 2010-2017 Google, Inc. http://angularjs.org
 License: MIT
*/
(function (s, d) {
    'use strict'; function J(d) { var k = []; w(k, B).chars(d); return k.join("") } var x = d.$$minErr("$sanitize"), C, k, D, E, p, B, F, G, w; d.module("ngSanitize", []).provider("$sanitize", function () {
        function g(a, e) { var c = {}, b = a.split(","), f; for (f = 0; f < b.length; f++) c[e ? p(b[f]) : b[f]] = !0; return c } function K(a) { for (var e = {}, c = 0, b = a.length; c < b; c++) { var f = a[c]; e[f.name] = f.value } return e } function H(a) {
            return a.replace(/&/g, "&amp;").replace(L, function (a) {
                var c = a.charCodeAt(0); a = a.charCodeAt(1); return "&#" + (1024 * (c -
                55296) + (a - 56320) + 65536) + ";"
            }).replace(M, function (a) { return "&#" + a.charCodeAt(0) + ";" }).replace(/</g, "&lt;").replace(/>/g, "&gt;")
        } function I(a) { for (; a;) { if (a.nodeType === s.Node.ELEMENT_NODE) for (var e = a.attributes, c = 0, b = e.length; c < b; c++) { var f = e[c], h = f.name.toLowerCase(); if ("xmlns:ns1" === h || 0 === h.lastIndexOf("ns1:", 0)) a.removeAttributeNode(f), c--, b-- } (e = a.firstChild) && I(e); a = t("nextSibling", a) } } function t(a, e) { var c = e[a]; if (c && F.call(e, c)) throw x("elclob", e.outerHTML || e.outerText); return c } var y = !1; this.$get =
        ["$$sanitizeUri", function (a) { y && k(n, z); return function (e) { var c = []; G(e, w(c, function (b, c) { return !/^unsafe:/.test(a(b, c)) })); return c.join("") } }]; this.enableSvg = function (a) { return E(a) ? (y = a, this) : y }; C = d.bind; k = d.extend; D = d.forEach; E = d.isDefined; p = d.$$lowercase; B = d.noop; G = function (a, e) {
            null === a || void 0 === a ? a = "" : "string" !== typeof a && (a = "" + a); var c = u(a); if (!c) return ""; var b = 5; do { if (0 === b) throw x("uinput"); b--; a = c.innerHTML; c = u(a) } while (a !== c.innerHTML); for (b = c.firstChild; b;) {
                switch (b.nodeType) {
                    case 1: e.start(b.nodeName.toLowerCase(),
                    K(b.attributes)); break; case 3: e.chars(b.textContent)
                } var f; if (!(f = b.firstChild) && (1 === b.nodeType && e.end(b.nodeName.toLowerCase()), f = t("nextSibling", b), !f)) for (; null == f;) { b = t("parentNode", b); if (b === c) break; f = t("nextSibling", b); 1 === b.nodeType && e.end(b.nodeName.toLowerCase()) } b = f
            } for (; b = c.firstChild;) c.removeChild(b)
        }; w = function (a, e) {
            var c = !1, b = C(a, a.push); return {
                start: function (a, h) {
                    a = p(a); !c && A[a] && (c = a); c || !0 !== n[a] || (b("<"), b(a), D(h, function (c, h) {
                        var d = p(h), g = "img" === a && "src" === d || "background" ===
                        d; !0 !== v[d] || !0 === m[d] && !e(c, g) || (b(" "), b(h), b('="'), b(H(c)), b('"'))
                    }), b(">"))
                }, end: function (a) { a = p(a); c || !0 !== n[a] || !0 === h[a] || (b("</"), b(a), b(">")); a == c && (c = !1) }, chars: function (a) { c || b(H(a)) }
            }
        }; F = s.Node.prototype.contains || function (a) { return !!(this.compareDocumentPosition(a) & 16) }; var L = /[\uD800-\uDBFF][\uDC00-\uDFFF]/g, M = /([^#-~ |!])/g, h = g("area,br,col,hr,img,wbr"), q = g("colgroup,dd,dt,li,p,tbody,td,tfoot,th,thead,tr"), l = g("rp,rt"), r = k({}, l, q), q = k({}, q, g("address,article,aside,blockquote,caption,center,del,dir,div,dl,figure,figcaption,footer,h1,h2,h3,h4,h5,h6,header,hgroup,hr,ins,map,menu,nav,ol,pre,section,table,ul")),
        l = k({}, l, g("a,abbr,acronym,b,bdi,bdo,big,br,cite,code,del,dfn,em,font,i,img,ins,kbd,label,map,mark,q,ruby,rp,rt,s,samp,small,span,strike,strong,sub,sup,time,tt,u,var")), z = g("circle,defs,desc,ellipse,font-face,font-face-name,font-face-src,g,glyph,hkern,image,linearGradient,line,marker,metadata,missing-glyph,mpath,path,polygon,polyline,radialGradient,rect,stop,svg,switch,text,title,tspan"), A = g("script,style"), n = k({}, h, q, l, r), m = g("background,cite,href,longdesc,src,xlink:href"), r = g("abbr,align,alt,axis,bgcolor,border,cellpadding,cellspacing,class,clear,color,cols,colspan,compact,coords,dir,face,headers,height,hreflang,hspace,ismap,lang,language,nohref,nowrap,rel,rev,rows,rowspan,rules,scope,scrolling,shape,size,span,start,summary,tabindex,target,title,type,valign,value,vspace,width"),
        l = g("accent-height,accumulate,additive,alphabetic,arabic-form,ascent,baseProfile,bbox,begin,by,calcMode,cap-height,class,color,color-rendering,content,cx,cy,d,dx,dy,descent,display,dur,end,fill,fill-rule,font-family,font-size,font-stretch,font-style,font-variant,font-weight,from,fx,fy,g1,g2,glyph-name,gradientUnits,hanging,height,horiz-adv-x,horiz-origin-x,ideographic,k,keyPoints,keySplines,keyTimes,lang,marker-end,marker-mid,marker-start,markerHeight,markerUnits,markerWidth,mathematical,max,min,offset,opacity,orient,origin,overline-position,overline-thickness,panose-1,path,pathLength,points,preserveAspectRatio,r,refX,refY,repeatCount,repeatDur,requiredExtensions,requiredFeatures,restart,rotate,rx,ry,slope,stemh,stemv,stop-color,stop-opacity,strikethrough-position,strikethrough-thickness,stroke,stroke-dasharray,stroke-dashoffset,stroke-linecap,stroke-linejoin,stroke-miterlimit,stroke-opacity,stroke-width,systemLanguage,target,text-anchor,to,transform,type,u1,u2,underline-position,underline-thickness,unicode,unicode-range,units-per-em,values,version,viewBox,visibility,width,widths,x,x-height,x1,x2,xlink:actuate,xlink:arcrole,xlink:role,xlink:show,xlink:title,xlink:type,xml:base,xml:lang,xml:space,xmlns,xmlns:xlink,y,y1,y2,zoomAndPan",
        !0), v = k({}, m, l, r), u = function (a, e) {
            function c(b) { b = "<remove></remove>" + b; try { var c = (new a.DOMParser).parseFromString(b, "text/html").body; c.firstChild.remove(); return c } catch (e) { } } function b(a) { d.innerHTML = a; e.documentMode && I(d); return d } var h; if (e && e.implementation) h = e.implementation.createHTMLDocument("inert"); else throw x("noinert"); var d = (h.documentElement || h.getDocumentElement()).querySelector("body"); d.innerHTML = '<svg><g onload="this.parentNode.remove()"></g></svg>'; return d.querySelector("svg") ?
            (d.innerHTML = '<svg><p><style><img src="</style><img src=x onerror=alert(1)//">', d.querySelector("svg img") ? c : b) : function (b) { b = "<remove></remove>" + b; try { b = encodeURI(b) } catch (c) { return } var e = new a.XMLHttpRequest; e.responseType = "document"; e.open("GET", "data:text/html;charset=utf-8," + b, !1); e.send(null); b = e.response.body; b.firstChild.remove(); return b }
        }(s, s.document)
    }).info({ angularVersion: "1.6.6-build.5421+sha.77b302a" }); d.module("ngSanitize").filter("linky", ["$sanitize", function (g) {
        var k = /((ftp|https?):\/\/|(www\.)|(mailto:)?[A-Za-z0-9._%+-]+@)\S*[^\s.;,(){}<>"\u201d\u2019]/i,
        p = /^mailto:/i, s = d.$$minErr("linky"), t = d.isDefined, y = d.isFunction, w = d.isObject, x = d.isString; return function (d, q, l) {
            function r(a) { a && m.push(J(a)) } function z(a, d) { var c, b = A(a); m.push("<a "); for (c in b) m.push(c + '="' + b[c] + '" '); !t(q) || "target" in b || m.push('target="', q, '" '); m.push('href="', a.replace(/"/g, "&quot;"), '">'); r(d); m.push("</a>") } if (null == d || "" === d) return d; if (!x(d)) throw s("notstring", d); for (var A = y(l) ? l : w(l) ? function () { return l } : function () { return {} }, n = d, m = [], v, u; d = n.match(k) ;) v = d[0], d[2] ||
            d[4] || (v = (d[3] ? "http://" : "mailto:") + v), u = d.index, r(n.substr(0, u)), z(v, d[0].replace(p, "")), n = n.substring(u + d[0].length); r(n); return g(m.join(""))
        }
    }])
})(window, window.angular);
//# sourceMappingURL=angular-sanitize.min.js.map
