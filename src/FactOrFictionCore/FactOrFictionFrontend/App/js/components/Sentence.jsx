import React from 'react';
import PropTypes from 'prop-types';
import { changeFocus } from '../utils/scroll';
import _ from '../../stylesheets/components/_Sentence.scss'

export default class Sentence extends React.Component {
    static propTypes = {
        id: PropTypes.string.isRequired,
        type: PropTypes.string.isRequired,
        content: PropTypes.string.isRequired,
        selectedEntryId: PropTypes.string.isRequired,
        selectEntry: PropTypes.func.isRequired,
        similarTokenIds: PropTypes.object.isRequired,
        fetchSimilarTokens: PropTypes.func.isRequired
    }

    render() {
        const { id, type, content, selectedEntryId } = this.props;
        const isSelected = id === selectedEntryId;
        const highlighted = type === 'OBJECTIVE' ? "highlighted" : "";

        return (
            <span id={id+"-sentence"}>
                <span
                    className={isSelected ? "selected" : highlighted}
                    onClick={() => this.handleClick()}
                >
                    {content}
                </span>
                &nbsp;
            </span>
        );
    }

    handleClick() {
        const { id, type, selectEntry, similarTokenIds, fetchSimilarTokens, selectedEntryId } = this.props;
        const isSelected = id === selectedEntryId;
        if (type === "OBJECTIVE") {
            selectEntry(id);
            if (!isSelected) { changeFocus(id, "sentence"); }
            if (!similarTokenIds.hasOwnProperty(id)) {
                fetchSimilarTokens(id);
            }
        }
    }
}
