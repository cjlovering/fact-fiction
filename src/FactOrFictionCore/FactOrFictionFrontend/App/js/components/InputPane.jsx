import React from 'react';
import PropTypes from 'prop-types';
import _ from '../../stylesheets/components/_InputPane.scss';
import { VIEW_RESULT } from '../constants/viewTypes';

export default class InputPane extends React.Component {
	constructor(props) {
	    super(props);
		this.state = {
			textEntry: "Enter some text (news article...)!"
		}
	}

    render() {
		const { changeView, fetchTextEntry } = this.props;
		const { textEntry } = this.state;
        return (
            <div>
				<div>
                    <textarea 
                        className="input-box"
						rows="10" 
						value={textEntry}
						onChange={e => this.setState({ textEntry: e.target.value })}
					/>
				</div>

                <button 
					onClick={() => {
						fetchTextEntry(textEntry);
						changeView(VIEW_RESULT);
					}}
					className="start-button ms-Button"
                    id="get-data-from-selection"
				>
                    <span className="ms-Button-label">Start</span>
				</button>
			</div>
        );
    }
}

InputPane.propTypes = {
	changeView: PropTypes.func.isRequired,
	fetchTextEntry: PropTypes.func.isRequired	
}