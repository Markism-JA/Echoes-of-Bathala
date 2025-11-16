#!/usr/bin/env bash

shopt -s globstar

BUILD_ROOT_DIR="build"
INDIVIDUAL_COMPILATION_RECURSIVE="true"
RECURSIVE_CHAPTER_BUILD="true"

compile_md_to_tex() {
    local md_file="$1"
    local source_chapter_root_dir="$2"

    local relative_md_path_from_chapter_root="${md_file#"$source_chapter_root_dir/"}"
    local build_output_dir="${BUILD_ROOT_DIR}/${source_chapter_root_dir}/$(dirname "$relative_md_path_from_chapter_root")"

    mkdir -p "$build_output_dir"

    local filename_no_ext="$(basename "$md_file" .md)"
    local tex_file="${build_output_dir}/${filename_no_ext}.tex"

    echo "Compiling ${md_file} to ${tex_file}..."
    pandoc "$md_file" \
        --top-level-division=chapter \
        --biblatex \
        -o "$tex_file"

    if [ $? -eq 0 ]; then
        echo "Successfully compiled ${md_file}"
    else
        echo "Error compiling ${md_file}" >&2
    fi
}

generate_include_tex() {
    local source_chapter_dir="$1"
    local build_chapter_dir="${BUILD_ROOT_DIR}/${source_chapter_dir}"
    local include_file_path="${build_chapter_dir}/include.tex"
    local md_files_found=()

    echo "Generating or updating ${include_file_path}..."

    mkdir -p "$build_chapter_dir"

    if [[ "$RECURSIVE_CHAPTER_BUILD" == "true" ]]; then
        mapfile -t md_files_found < <(find "$source_chapter_dir" -type f -name "*.md" | sort)
    else
        mapfile -t md_files_found < <(find "$source_chapter_dir" -maxdepth 1 -type f -name "*.md" | sort)
    fi

    {
        echo "% Auto-generated include.tex for ${source_chapter_dir}"
        echo "% All \\input paths are absolute from the project root (where main.tex is)."
        echo ""
        for md_file in "${md_files_found[@]}"; do
            local final_tex_path_from_root="${BUILD_ROOT_DIR}/${md_file%.md}.tex"
            echo "\input{${final_tex_path_from_root}}"
        done
    } >"$include_file_path"

    echo "Generated ${include_file_path} with $(wc -l <"$include_file_path") entries."
}

compile_chapter_directory() {
    local source_chapter_dir="$1"
    local interactive="$2"
    local force_regen="$3"

    local build_chapter_dir="${BUILD_ROOT_DIR}/${source_chapter_dir}"
    local chapter_output_file="${build_chapter_dir}/$(basename "$source_chapter_dir").tex"
    local include_file_path="${build_chapter_dir}/include.tex"

    echo -e "\n--- Processing Chapter Directory: ${source_chapter_dir} (Output to: ${build_chapter_dir}) ---"

    echo "Processing individual Markdown files for source: ${source_chapter_dir}..."
    local md_search_pattern_glob="${source_chapter_dir}/**.md"

    for md_file in $md_search_pattern_glob; do
        if [ -f "$md_file" ]; then
            if [[ "$interactive" == "true" ]]; then
                read -p "Compile this file? [${md_file}] (y/N) " choice
                case "$choice" in
                y | Y) compile_md_to_tex "$md_file" "$source_chapter_dir" ;;
                *) echo "--> Ignoring file." ;;
                esac
            else
                compile_md_to_tex "$md_file" "$source_chapter_dir"
            fi
        fi
    done

    if [[ "$force_regen" == "true" ]]; then
        echo "--> Force flag detected. Regenerating include.tex."
        generate_include_tex "$source_chapter_dir"
    elif [ ! -f "$include_file_path" ]; then
        echo "--> include.tex not found. Auto-generating it."
        generate_include_tex "$source_chapter_dir"
    elif [[ "$interactive" == "true" ]]; then
        read -p "--> include.tex already exists. Overwrite? (y/N) " choice
        case "$choice" in
        y | Y)
            echo "--> Overwriting include.tex as requested."
            generate_include_tex "$source_chapter_dir"
            ;;
        *)
            echo "--> Keeping existing include.tex."
            ;;
        esac
    else
        echo "--> Found existing include.tex. Skipping generation in non-interactive mode."
    fi

    echo "Creating main chapter file: ${chapter_output_file}"
    local include_file_path_from_root="${build_chapter_dir}/include.tex"
    echo "\input{${include_file_path_from_root}}" >"$chapter_output_file"
    echo "Successfully created main chapter file."

    echo "--- Finished Processing Chapter Directory: ${source_chapter_dir} ---"
}

INTERACTIVE_MODE="false"
FORCE_REGEN="false"

while [[ "$#" -gt 0 ]]; do
    case "$1" in
    --interactive)
        INTERACTIVE_MODE="true"
        shift
        ;;
    --force-include-regen)
        FORCE_REGEN="true"
        shift
        ;;
    *)
        SELECTED_CHAPTER_DIRS+=("$1")
        shift
        ;;
    esac
done

echo "Starting compilation..."
echo "- Interactive Mode: $INTERACTIVE_MODE"
echo "- Force include.tex Regeneration: $FORCE_REGEN"
echo "- Output Directory: ${BUILD_ROOT_DIR}/"

if [ ${#SELECTED_CHAPTER_DIRS[@]} -eq 0 ]; then
    declare -a CHAPTER_DIRS=("01_Introduction" "02_Methodology")
else
    declare -a CHAPTER_DIRS=("${SELECTED_CHAPTER_DIRS[@]}")
fi

mkdir -p "$BUILD_ROOT_DIR"

for dir in "${CHAPTER_DIRS[@]}"; do
    if [ -d "$dir" ]; then
        compile_chapter_directory "$dir" "$INTERACTIVE_MODE" "$FORCE_REGEN"
    else
        echo "Warning: Main chapter directory not found: ${dir}. Skipping." >&2
    fi
done

echo -e "\n--- All compilation tasks finished. ---"
