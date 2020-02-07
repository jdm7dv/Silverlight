#!/bin/bash
OUTFILE=$2
LIBFILE=$1
PROTECTOR=$(echo _${OUTFILE}_ | tr [a-z].- [A-Z]__)
cat > $OUTFILE << MAGIG_HEADER_LOVE
/*
 * Copyright (C) 2007 Novell, Inc (http://www.novell.com)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
 * NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 * Authors:
 *            Larry Ewing <lewing@novell.com> (via script)
 *
 */

/*
 *
 * AUTOGENERATED ! This file was autogenerated using the tools/library-embed/library-embed.sh script. Do not edit by hand!
 *
 */

MAGIG_HEADER_LOVE

echo \#ifndef ${PROTECTOR} >> $OUTFILE
echo \#define ${PROTECTOR} >> $OUTFILE

nm $LIBFILE | awk '/ T \_/ { if ($3 != "__i686.get_pc_thunk.bx" && $3 != "__i686.get_pc_thunk.cx") printf "#define %s _moonlight%s\n",$3,$3} / T [^_]/ {printf "#define %s moonlight_%s\n",$3,$3}' >> $OUTFILE

echo "#endif" >> $OUTFILE
