import React from 'react';
import PropTypes from 'prop-types';
import _ from '../../stylesheets/components/_Sentence.scss'

export default class Sentence extends React.Component {
    render() {
        const { id, type, sentence, selectedEntryId } = this.props;
        const isSelected = id === selectedEntryId;
        const highlighted = type === 'OBJECTIVE' ? "highlighted" : "";

        return (
            <span>
                <span
                    className={isSelected ? "selected" : highlighted}
                    onClick={() => this.handleClick()}
                >
                    {sentence}
                </span>
                &nbsp;
            </span>
        );
    }

    handleClick() {
        const { id, type, selectEntry } = this.props;

        if (type === "OBJECTIVE") {
            selectEntry(id);
        }
    }
}

Sentence.propTypes = {
    id: PropTypes.string.isRequired,
    type: PropTypes.string.isRequired,
    sentence: PropTypes.string.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    selectEntry: PropTypes.func.isRequired
}