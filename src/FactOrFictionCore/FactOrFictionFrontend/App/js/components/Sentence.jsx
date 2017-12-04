import React from 'react';
import PropTypes from 'prop-types';
import _ from '../../stylesheets/components/_Sentence.scss'

export default class Sentence extends React.Component {
    static propTypes = {
        id: PropTypes.string.isRequired,
        type: PropTypes.string.isRequired,
        content: PropTypes.string.isRequired,
        selectedEntryId: PropTypes.string.isRequired,
        selectEntry: PropTypes.func.isRequired,
        similarSentenceIds: PropTypes.object.isRequired,
        fetchSimilarSentences: PropTypes.func.isRequired
    }

    render() {
        const { id, type, content, selectedEntryId } = this.props;
        const isSelected = id === selectedEntryId;
        const highlighted = type === 'OBJECTIVE' ? "highlighted" : "";

        return (
            <span>
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
        const { id, type, selectEntry, similarSentenceIds, fetchSimilarSentences } = this.props;
        if (type === "OBJECTIVE") {
            selectEntry(id);
            if (!similarSentenceIds.hasOwnProperty(id)) {
                fetchSimilarSentences(id);
            }
        }
    }
}
